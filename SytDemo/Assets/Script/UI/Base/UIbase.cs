namespace SytUI
{
    using System;
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using Object = UnityEngine.Object;
    using System.Text;

    #region define

    public enum UIType
    {
        Normal,
        Fixed,
        PopUp,
        None,      //独立的窗口
    }

    public enum UIMode
    {
        DoNothing,
        HideOther,     // 闭其他界面
        NeedBack,      // 点击返回按钮关闭当前,不关闭其他界面(需要调整好层级关系)
        NoNeedBack,    // 关闭TopBar,关闭其他界面,不加入backSequence队列
    }

    public enum UICollider
    {
        None,      // 显示该界面不包含碰撞背景
        Normal,    // 碰撞透明背景
        WithBg,    // 碰撞非透明背景
    }
    #endregion

    /// <summary>
    /// UI基类
    /// 所有UI需继承UIbase
    /// </summary>
    public abstract class UIbase
    {
        public string name = string.Empty;

        public int id = -1;

        public UIType type = UIType.Normal;

        public UIMode mode = UIMode.DoNothing;

        public UICollider collider = UICollider.None;

        public string uiPath = string.Empty;

        public GameObject gameObject;
        public Transform transform;

        private static Dictionary<string, UIbase> m_allPages;
        public static Dictionary<string, UIbase> allPages
        { get { return m_allPages; } }

        private static List<UIbase> m_currentPageNodes;
        public static List<UIbase> currentPageNodes
        { get { return m_currentPageNodes; } }

        private bool isAsyncUI = false;

        protected bool isActived = false;

        private object m_data = null;
        protected object data { get { return m_data; } }

        public static Func<string, Object> delegateSyncLoadUI = null;
        public static Action<string, Action<Object>> delegateAsyncLoadUI = null;

        #region virtual api

        /// <summary>
        /// 实例化页面时候会执行1次
        /// </summary>
        /// <param name="go"></param>
        public virtual void Awake(GameObject go) { }

        /// <summary>
        /// 每次都会刷新，当调用 ShowPage
        /// </summary>
        public virtual void Refresh() { }

        /// <summary>
        /// 可以实现自己的显示页面方式，默认是 this.gameObject.SetActive(true)
        /// </summary>
        public virtual void Active()
        {
            this.gameObject.SetActive(true);
            isActived = true;
        }

        /// <summary>
        /// 可以实现指定的隐藏或默认是Destroy默认 this.gameObject.SetActive(false)
        /// </summary>
        public virtual void Hide()
        {
            this.gameObject.SetActive(false);
            isActived = false;
            this.m_data = null;
        }

        #endregion

        #region internal api

        private UIbase() { }
        public UIbase(UIType type, UIMode mod, UICollider col)
        {
            this.type = type;
            this.mode = mod;
            this.collider = col;
            this.name = this.GetType().ToString();

            //when create one page.
            //bind special delegate .
            UIbind.Bind();
            //Debug.LogWarning("[UI] create page:" + ToString());
        }

        /// <summary>
        /// Sync Show UI Logic
        /// </summary>
        protected void Show()
        {
            //1:instance UI
            if(this.gameObject == null && string.IsNullOrEmpty(uiPath) == false)
            {
                GameObject go = null;
                if(delegateSyncLoadUI != null)
                {
                    Object o = delegateSyncLoadUI(uiPath);
                    go = o != null ? GameObject.Instantiate(o) as GameObject : null;
                }
                else
                {
                    go = GameObject.Instantiate(Resources.Load(uiPath)) as GameObject;
                }

                //protected.
                if(go == null)
                {
                    Debug.LogError("[UI] Cant sync load your ui prefab.");
                    return;
                }

                AnchorUIGameObject(go);

                //after instance should awake init.
                Awake(go);

                //mark this ui sync ui
                isAsyncUI = false;
            }

            //:animation or init when active.
            Active();

            //:refresh ui component.
            Refresh();

            //:popup this node to top if need back.
            PopNode(this);
        }

        /// <summary>
        /// Async Show UI Logic
        /// </summary>
        protected void Show(Action callback)
        {
            UIManager.instance.StartCoroutine(AsyncShow(callback));
        }

        IEnumerator AsyncShow(Action callback)
        {
            //1:Instance UI
            //FIX:support this is manager multi gameObject,instance by your self.
            if(this.gameObject == null && string.IsNullOrEmpty(uiPath) == false)
            {
                GameObject go = null;
                bool _loading = true;
                delegateAsyncLoadUI(uiPath, (o) =>
                {
                    go = o != null ? GameObject.Instantiate(o) as GameObject : null;
                    AnchorUIGameObject(go);
                    Awake(go);
                    isAsyncUI = true;
                    _loading = false;

                //:animation active.
                Active();

                //:refresh ui component.
                Refresh();

                //:popup this node to top if need back.
                PopNode(this);

                    if(callback != null) callback();
                });

                float _t0 = Time.realtimeSinceStartup;
                while(_loading)
                {
                    if(Time.realtimeSinceStartup - _t0 >= 10.0f)
                    {
                        Debug.LogError("[UI] WTF async load your ui prefab timeout!");
                        yield break;
                    }
                    yield return null;
                }
            }
            else
            {
                //:animation active.
                Active();

                //:refresh ui component.
                Refresh();

                //:popup this node to top if need back.
                PopNode(this);

                if(callback != null) callback();
            }
        }

        internal bool CheckIfNeedBack()
        {
            if(type == UIType.Fixed || type == UIType.PopUp || type == UIType.None) return false;
            else if(mode == UIMode.NoNeedBack || mode == UIMode.DoNothing) return false;
            return true;
        }

        protected void AnchorUIGameObject(GameObject ui)
        {
            if(UIManager.instance == null || ui == null) return;

            this.gameObject = ui;
            this.transform = ui.transform;

            //check if this is ugui or (ngui)?
            Vector3 anchorPos = Vector3.zero;
            Vector2 sizeDel = Vector2.zero;
            Vector3 scale = Vector3.one;
            if(ui.GetComponent<RectTransform>() != null)
            {
                anchorPos = ui.GetComponent<RectTransform>().anchoredPosition;
                sizeDel = ui.GetComponent<RectTransform>().sizeDelta;
                scale = ui.GetComponent<RectTransform>().localScale;
            }
            else
            {
                anchorPos = ui.transform.localPosition;
                scale = ui.transform.localScale;
            }

            //Debug.Log("anchorPos:" + anchorPos + "|sizeDel:" + sizeDel);

            if(type == UIType.Fixed)
            {
                ui.transform.SetParent(UIManager.instance.FixedRoot);
            }
            else if(type == UIType.Normal)
            {
                ui.transform.SetParent(UIManager.instance.NormalRoot);
            }
            else if(type == UIType.PopUp)
            {
                ui.transform.SetParent(UIManager.instance.PopupRoot);
            }


            if(ui.GetComponent<RectTransform>() != null)
            {
                ui.GetComponent<RectTransform>().anchoredPosition = anchorPos;
                ui.GetComponent<RectTransform>().sizeDelta = sizeDel;
                ui.GetComponent<RectTransform>().localScale = scale;
            }
            else
            {
                ui.transform.localPosition = anchorPos;
                ui.transform.localScale = scale;
            }
        }

        public override string ToString()
        {
            return ">Name:" + name + ",ID:" + id + ",Type:" + type.ToString() + ",ShowMode:" + mode.ToString() + ",Collider:" + collider.ToString();
        }

        public bool isActive()
        {
            //fix,if this page is not only one gameObject
            //so,should check isActived too.
            bool ret = gameObject != null && gameObject.activeSelf;
            return ret || isActived;
        }

        #endregion

        #region static api

        private static bool CheckIfNeedBack(UIbase page)
        {
            return page != null && page.CheckIfNeedBack();
        }

        /// <summary>
        /// 将目标节点设置在顶部
        /// </summary>
        private static void PopNode(UIbase page)
        {
            if(m_currentPageNodes == null)
            {
                m_currentPageNodes = new List<UIbase>();
            }

            if(page == null)
            {
                Debug.LogError("[UI] page popup is null.");
                return;
            }

            if(CheckIfNeedBack(page) == false)
            {
                return;
            }

            bool _isFound = false;
            for(int i = 0; i < m_currentPageNodes.Count; i++)
            {
                if(m_currentPageNodes[i].Equals(page))
                {
                    m_currentPageNodes.RemoveAt(i);
                    m_currentPageNodes.Add(page);
                    _isFound = true;
                    break;
                }
            }

            if(!_isFound)
            {
                m_currentPageNodes.Add(page);
            }

            HideOldNodes();
        }

        private static void HideOldNodes()
        {
            if(m_currentPageNodes.Count < 0) return;
            UIbase topPage = m_currentPageNodes[m_currentPageNodes.Count - 1];
            if(topPage.mode == UIMode.HideOther)
            {
                //form bottm to top.
                for(int i = m_currentPageNodes.Count - 2; i >= 0; i--)
                {
                    if(m_currentPageNodes[i].isActive())
                        m_currentPageNodes[i].Hide();
                }
            }
        }

        public static void ClearNodes()
        {
            m_currentPageNodes.Clear();
        }

        private static void ShowPage<T>(Action callback, object pageData, bool isAsync) where T : UIbase, new()
        {
            Type t = typeof(T);
            string pageName = t.ToString();

            if(m_allPages != null && m_allPages.ContainsKey(pageName))
            {
                ShowPage(pageName, m_allPages[pageName], callback, pageData, isAsync);
            }
            else
            {
                T instance = new T();
                ShowPage(pageName, instance, callback, pageData, isAsync);
            }
        }

        private static void ShowPage(string pageName, UIbase pageInstance, Action callback, object pageData, bool isAsync)
        {
            if(string.IsNullOrEmpty(pageName) || pageInstance == null)
            {
                Debug.LogError("[UI] show page error with :" + pageName + " maybe null instance.");
                return;
            }

            if(m_allPages == null)
            {
                m_allPages = new Dictionary<string, UIbase>();
            }

            UIbase page = null;
            if(m_allPages.ContainsKey(pageName))
            {
                page = m_allPages[pageName];
            }
            else
            {
                m_allPages.Add(pageName, pageInstance);
                page = pageInstance;
            }

            //if active before,wont active again.
            //if (page.isActive() == false)
            {
                //before show should set this data if need. maybe.!!
                page.m_data = pageData;

                if(isAsync)
                    page.Show(callback);
                else
                    page.Show();
            }
        }

        public static void ShowPage<T>() where T : UIbase, new()
        {
            ShowPage<T>(null, null, false);
        }

        public static void ShowPage<T>(object pageData) where T : UIbase, new()
        {
            ShowPage<T>(null, pageData, false);
        }

        public static void ShowPage(string pageName, UIbase pageInstance)
        {
            ShowPage(pageName, pageInstance, null, null, false);
        }

        public static void ShowPage(string pageName, UIbase pageInstance, object pageData)
        {
            ShowPage(pageName, pageInstance, null, pageData, false);
        }

        public static void ShowPage<T>(Action callback) where T : UIbase, new()
        {
            ShowPage<T>(callback, null, true);
        }

        public static void ShowPage<T>(Action callback, object pageData) where T : UIbase, new()
        {
            ShowPage<T>(callback, pageData, true);
        }

        public static void ShowPage(string pageName, UIbase pageInstance, Action callback)
        {
            ShowPage(pageName, pageInstance, callback, null, true);
        }

        public static void ShowPage(string pageName, UIbase pageInstance, Action callback, object pageData)
        {
            ShowPage(pageName, pageInstance, callback, pageData, true);
        }

        public static void ClosePage()
        {
            //Debug.Log("Back&Close PageNodes Count:" + m_currentPageNodes.Count);

            if(m_currentPageNodes == null || m_currentPageNodes.Count <= 1) return;

            UIbase closePage = m_currentPageNodes[m_currentPageNodes.Count - 1];
            m_currentPageNodes.RemoveAt(m_currentPageNodes.Count - 1);

            //show older page.
            //TODO:Sub pages.belong to root node.
            if(m_currentPageNodes.Count > 0)
            {
                UIbase page = m_currentPageNodes[m_currentPageNodes.Count - 1];
                if(page.isAsyncUI)
                    ShowPage(page.name, page, () =>
                    {
                        closePage.Hide();
                    });
                else
                {
                    ShowPage(page.name, page);

                    //after show to hide().
                    closePage.Hide();
                }
            }
        }

        public static void ClosePage(UIbase target)
        {
            if(target == null) return;
            if(target.isActive() == false)
            {
                if(m_currentPageNodes != null)
                {
                    for(int i = 0; i < m_currentPageNodes.Count; i++)
                    {
                        if(m_currentPageNodes[i] == target)
                        {
                            m_currentPageNodes.RemoveAt(i);
                            break;
                        }
                    }
                    return;
                }
            }

            if(m_currentPageNodes != null && m_currentPageNodes.Count >= 1 && m_currentPageNodes[m_currentPageNodes.Count - 1] == target)
            {
                m_currentPageNodes.RemoveAt(m_currentPageNodes.Count - 1);

                //show older page.
                //TODO:Sub pages.belong to root node.
                if(m_currentPageNodes.Count > 0)
                {
                    UIbase page = m_currentPageNodes[m_currentPageNodes.Count - 1];
                    if(page.isAsyncUI)
                        ShowPage(page.name, page, () =>
                        {
                            target.Hide();
                        });
                    else
                    {
                        ShowPage(page.name, page);
                        target.Hide();
                    }

                    return;
                }
            }
            else if(target.CheckIfNeedBack())
            {
                for(int i = 0; i < m_currentPageNodes.Count; i++)
                {
                    if(m_currentPageNodes[i] == target)
                    {
                        m_currentPageNodes.RemoveAt(i);
                        target.Hide();
                        break;
                    }
                }
            }

            target.Hide();
        }

        public static void ClosePage<T>() where T : UIbase
        {
            Type t = typeof(T);
            string pageName = t.ToString();

            if(m_allPages != null && m_allPages.ContainsKey(pageName))
            {
                ClosePage(m_allPages[pageName]);
            }
            else
            {
                Debug.LogError(pageName + "havnt show yet!");
            }
        }

        public static void ClosePage(string pageName)
        {
            if(m_allPages != null && m_allPages.ContainsKey(pageName))
            {
                ClosePage(m_allPages[pageName]);
            }
            else
            {
                Debug.LogError(pageName + " havnt show yet!");
            }
        }

        #endregion

    }
}