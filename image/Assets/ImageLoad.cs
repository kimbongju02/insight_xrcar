using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageLoad : MonoBehaviour
{
    [SerializeField]
    RawImage rawImage;
    [SerializeField]
    int index = 1;

    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = 15;
    }

    // Update is called once per frame
    void Update()
    {
        //�̹��� ��ȣ
        string path = index.ToString();

        //�̹��� �ҷ�����
        Texture2D texture = new Texture2D(1920, 1280);
        texture = Resources.Load<Texture2D>(path);

        //�̹��� ����
        rawImage.texture = null;
        Resources.UnloadUnusedAssets();
        rawImage.texture = texture;

        index = index + 1 > 10 ? 1 : index + 1;
    }
}
