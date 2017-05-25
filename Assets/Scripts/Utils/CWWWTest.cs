using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;


//图片下载测试
public class CWWWTest : MonoBehaviour
{


    WWW www;                     //请求
    string filePath;             //保存的文件路径
    Texture2D texture2D;         //下载的图片
    public Transform m_tSprite;  //场景中的一个Sprite

    void Start()
    {
        //保存路径
        filePath = Application.dataPath + "/Resources/picture.jpg";
    }

    public void onBtnClick() {
        Debug.Log("开始下载");
        StartCoroutine(LoadImg());
    }

    IEnumerator LoadImg()
    {
        //开始下载图片
        www = new WWW("http://ym.zdmimg.com/201512/28/5680a3aa9a8f7.jpg_d200.jpg");
        yield return www;


        //下载完成，保存图片到路径filePath
        texture2D = www.texture;
        byte[] bytes = texture2D.EncodeToPNG();
        File.WriteAllBytes(filePath, bytes);


        //将图片赋给场景上的Sprite
        Sprite tempSp = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0, 0));
        m_tSprite.GetComponent<Image>().sprite = tempSp;
        Debug.Log("加载完成");

    }
}