using UnityEngine;
using System.Collections;

public abstract class NewBase : MonoBehaviour
{
    private object m_data = null;
    protected object data { get { return m_data; } }

    public virtual void Init() { }
    public virtual void OnStart() { }
    public virtual void Refresh() { }
    public virtual void Hide() { }

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        OnStart();
    }

    private void OnEnable()
    {
        Refresh();
    }

    private void OnDisable()
    {
        Hide();
    }
}
