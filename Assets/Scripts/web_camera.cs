using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;

public class web_camera : MonoBehaviour
{
    [SerializeField] private int m_WebCamIndex = 1;
    private Texture2D texture2D;
    private VideoCapture videoCapture;
    private Mat mat;
    private Mat grey;

    // Use this for initialization
    void Start()
    {
        videoCapture = new VideoCapture(m_WebCamIndex);
        texture2D = new Texture2D(videoCapture.FrameWidth, videoCapture.FrameHeight, TextureFormat.RGB24, false);
        GetComponent<Renderer>().material.mainTexture = texture2D;
        mat = new Mat();
        grey = new Mat();
    }

    // Update is called once per frame
    void Update()
    {
        videoCapture.Read(mat);
        Cv2.CvtColor(mat, grey, ColorConversionCodes.BGR2GRAY);
        texture2D.LoadImage(grey.ImEncode());
    }
    /*
    private void OnDestroy()
    {
        videoCapture.Dispose();
        videoCapture = null;
    }
    */
}