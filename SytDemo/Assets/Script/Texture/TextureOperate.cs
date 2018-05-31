using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TextureOperate : MonoBehaviour
{
    [Range(0, 1)]
    public float MoveTextureSpeed = 0.1f;
    public Texture2D OneTexture;
    public Texture2D TwoTexture;
    public Texture2D ThreeTexture;
    public Texture2D FourTexture;

    private Mesh mesh;
    private Material material;
    private Vector2 offset;
    private Ray ray;
    private RaycastHit rayHit;

    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        material = GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out rayHit))
            {
                if (rayHit.collider.gameObject == gameObject)
                {
                    float tempAngle = material.GetFloat("_RotateAngle");
                    tempAngle += 45;
                    material.SetFloat("_RotateAngle", tempAngle);
                }
            }
        }
        if (Input.GetMouseButton(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out rayHit))
            {
                if (rayHit.collider.gameObject == gameObject)
                {
                    float angle = material.GetFloat("_RotateAngle") % 360;
                    offset.x = material.GetFloat("_CenterX");
                    offset.y = material.GetFloat("_CenterY");

                    #region 角度处理
                    if (angle == 0)
                    {
                        offset.x += (Input.GetAxis("Mouse X") * Mathf.Cos(angle) + Input.GetAxis("Mouse Y") * Mathf.Sin(angle)) * MoveTextureSpeed;
                        offset.y += (Input.GetAxis("Mouse Y") * Mathf.Cos(angle) - Input.GetAxis("Mouse X") * Mathf.Sin(angle)) * MoveTextureSpeed;
                    }
                    else if (angle == 45)
                    {
                        offset.x += (Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y")) * 0.7071f * MoveTextureSpeed;
                        offset.y += (Input.GetAxis("Mouse Y") - Input.GetAxis("Mouse X")) * 0.7071f * MoveTextureSpeed;
                    }
                    else if (angle == 90)
                    {
                        offset.x += Input.GetAxis("Mouse Y") * MoveTextureSpeed;
                        offset.y -= Input.GetAxis("Mouse X") * MoveTextureSpeed;
                    }
                    else if (angle == 135)
                    {
                        angle -= 90;
                        offset.x += (-Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y")) * 0.7071f * MoveTextureSpeed;
                        offset.y += (-Input.GetAxis("Mouse Y") - Input.GetAxis("Mouse X")) * 0.7071f * MoveTextureSpeed;
                    }
                    else if (angle == 180)
                    {
                        angle -= 90;
                        offset.x -= Input.GetAxis("Mouse X") * MoveTextureSpeed;
                        offset.y -= Input.GetAxis("Mouse Y") * MoveTextureSpeed;
                    }
                    else if (angle == 225)
                    {
                        angle -= 180;
                        offset.x -= (Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y")) * 0.7071f * MoveTextureSpeed;
                        offset.y -= (Input.GetAxis("Mouse Y") - Input.GetAxis("Mouse X")) * 0.7071f * MoveTextureSpeed;
                    }
                    else if (angle == 270)
                    {
                        angle -= 180;
                        offset.x -= Input.GetAxis("Mouse Y") * MoveTextureSpeed;
                        offset.y += Input.GetAxis("Mouse X") * MoveTextureSpeed;
                    }
                    else if (angle == 315)
                    {
                        angle -= 180;
                        offset.x -= (-Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y")) * 0.7071f * MoveTextureSpeed;
                        offset.y -= (-Input.GetAxis("Mouse Y") - Input.GetAxis("Mouse X")) * 0.7071f * MoveTextureSpeed;
                    }
                    #endregion

                    material.SetFloat("_UA", offset.x + 0.5f);
                    material.SetFloat("_UB", offset.y + 0.5f);
                    material.SetFloat("_CenterX", offset.x);
                    material.SetFloat("_CenterY", offset.y);

                }
            }
        }
    }

    public void Btn_Reset()
    {
        material.SetFloat("_CenterX", 0.5f);
        material.SetFloat("_CenterY", 0.5f);
        material.SetFloat("_UA", 0.5f);
        material.SetFloat("_UB", 0.5f);
        material.SetFloat("_RotateAngle", 180);
    }
    public void SetOne()
    {
        Texture2D[] temp = { OneTexture };
        material.mainTexture = MergeImage.instance.Merge(temp);
    }
    public void SetTwo()
    {
        Texture2D[] temp = { OneTexture, TwoTexture };
        material.mainTexture = MergeImage.instance.Merge(temp);
    }
    public void SetFour()
    {
        Texture2D[] temp = { OneTexture, TwoTexture, ThreeTexture, FourTexture };
        material.mainTexture = MergeImage.instance.Merge2Row(temp);
    }

    Texture2D TextureRotate(Texture2D texture, float eulerAngles)
    {
        int x;
        int y;
        float phi = eulerAngles / (180 / Mathf.PI);
        float sn = Mathf.Sin(phi);
        float cs = Mathf.Cos(phi);
        Color32[] arr = texture.GetPixels32();
        Color32[] arr2 = new Color32[arr.Length];
        int W = texture.width;
        int H = texture.height;
        int xc = W / 2;
        int yc = H / 2;

        for (int j = 0; j < H; j++)
        {
            for (int i = 0; i < W; i++)
            {
                arr2[j * W + i] = new Color32(0, 0, 0, 0);

                x = (int)(cs * (i - xc) + sn * (j - yc) + xc);
                y = (int)(-sn * (i - xc) + cs * (j - yc) + yc);

                if ((x > -1) && (x < W) && (y > -1) && (y < H))
                {
                    arr2[j * W + i] = arr[y * W + x];
                }
            }
        }

        Texture2D newImg = new Texture2D(W, H);
        newImg.SetPixels32(arr2);
        newImg.Apply();

        return newImg;
    }

    void RotateUV(float rotatespeed)
    {
        List<Vector2> uvs = new List<Vector2>(mesh.uv);


        float speed = 0;
        speed = rotatespeed * Mathf.Deg2Rad;


        for (int i = 0; i < uvs.Count; i++)
        {
            Vector2 uv = uvs[i] - new Vector2(0.5f, 0.5f);
            uv = new Vector2(uv.x * Mathf.Cos(speed) - uv.y * Mathf.Sin(speed),
                              uv.x * Mathf.Sin(speed) + uv.y * Mathf.Cos(speed));
            uv += new Vector2(0.5f, 0.5f);
            uvs[i] = uv;
        }
        mesh.SetUVs(0, uvs);
    }

    void MoveUV(float MoveSpeed)
    {
        List<Vector2> uvs = new List<Vector2>(mesh.uv);
        for (int i = 0; i < uvs.Count; i++)
        {
            offset = uvs[i];
            offset.x += Input.GetAxis("Mouse X") * MoveSpeed;
            offset.y += Input.GetAxis("Mouse Y") * MoveSpeed;
            uvs[i] = offset;
        }
        mesh.SetUVs(0, uvs);
    }
}
