using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;
using OpenCvSharp.Aruco;

public class opencv_test : MonoBehaviour {
    private int source = 0; //カメラのインデックス

    private VideoCapture cam;
    public Mat cam_frame;

    private Texture2D cam_Texture;

    /* ArUcoの設定 */
    //ARマーカ辞書の設定
    private const PredefinedDictionaryName dictName = PredefinedDictionaryName.Dict4X4_50;
    private Dictionary ar_dict;
    //検出アルゴリズムの設定
    private DetectorParameters detect_param;

    // Use this for initialization
    void Start () {
        cam = new VideoCapture(source);
        cam.Set(CaptureProperty.ConvertRgb, 3);
        cam_frame = new Mat();

        ar_dict = CvAruco.GetPredefinedDictionary(dictName);
        detect_param = DetectorParameters.Create();

        cam_Texture = new Texture2D(
            cam.FrameWidth,    //カメラのwidth
            cam.FrameHeight,    //カメラのheight
            TextureFormat.RGB24, //フォーマットを指定
            false               //ミニマップ設定
        );
        this.GetComponent<Renderer>().material.mainTexture = cam_Texture;
    }
	
	// Update is called once per frame
	void Update () {
        Point2f[][] maker_corners;  //ARマーカのカドの座標
        int[] maker_ids;            //検出されたARマーカのID
        Point2f[][] reject_points;

        cam.Read(cam_frame);//フレームの更新

        //ARマーカの検出
        CvAruco.DetectMarkers(cam_frame, ar_dict, out maker_corners, out maker_ids, detect_param, out reject_points);

        //キャリブレーションデータを設定
        double[,] array3_3 = new double[3, 3] { { 548.8, 0.0, 332.1 }, { 0.0, 549.8, 236.2 }, { 0.0, 0.0, 1.0 } };
        Mat cameraMatrix = new Mat(3,3, MatType.CV_32FC1, array3_3);

        double[,] array1_5 = new double[1, 5] { { -0.0886, 0.4802, -0.0026, 0.0019 , -0.8238 } };
        Mat distortionCoefficients = new Mat(1, 5, MatType.CV_32FC1, array1_5);

        
        
        
        
        Mat rvec = new Mat();
        Mat tvec = new Mat();
        

        

        //VectorOfVec3f vector3_rvec = new VectorOfVec3f();
        //IEnumerable<VectorOfVec3f> vector3_rvec = new IEnumerable<VectorOfVec3f>;
        



        if (maker_ids.Length != 0)
        {
            //検出されたマーカ情報の描画
            CvAruco.DrawDetectedMarkers(cam_frame, maker_corners, maker_ids, new Scalar(0, 255, 0));
            //Debug.Log("Find");

            /*
            Mat rvec = new Mat(3, maker_ids.Length, MatType.CV_32FC1);
            Mat tvec = new Mat(3, maker_ids.Length, MatType.CV_32FC1);
            */
            
            /*
            Debug.Log(tvec.Rows);
            Debug.Log(tvec.Cols);
            Debug.Log(tvec.Size());
            Debug.Log(tvec.Dims());
            */

            CvAruco.EstimatePoseSingleMarkers(maker_corners, (float)1.8, cameraMatrix, distortionCoefficients, rvec, tvec);


            //マーカまでの距離
            /*
            //1
            Debug.Log(tvec.Rows);
            //1
            Debug.Log(tvec.Cols);
            //1:1
            Debug.Log(tvec.Size());
            //2
            Debug.Log(tvec.Dims());
            //3=#define CV_16S
            Debug.Log(tvec.Channels());
            */

            
            Vec3d vec3D = tvec.At<Vec3d>(0, 0);

            double z = vec3D[2];

            Debug.Log(z);
            
            
            /*
            Debug.Log(tvec.At<Vec3d>(0, 0)[2]);
            */

            //Debug.Log(tvec.Data[0]);
            //Debug.Log(tvec.At<Vec3b>(1, 1)[2]);

            //Debug.Log(tvec[new OpenCvSharp.Rect(0, 1,0,1)]);


            /*
            Debug.Log(rvec.Rows);
            Debug.Log(rvec.Cols);
            Debug.Log(rvec.Size());
            Debug.Log(rvec.Dims());
            */

            /*
            //Debug.Log(cameraMatrix.Size());
            //1
            Debug.Log(distortionCoefficients.Rows);
            //5
            Debug.Log(distortionCoefficients.Cols);
            Debug.Log(distortionCoefficients.Size());
            */

            /*
            //1channelはdoubleできれいに取れる
            Debug.Log(distortionCoefficients.At<double>(0,0));
            //1
            Debug.Log(distortionCoefficients.Channels());
            */

            //Debug.Log(tvec.)
            //24
            //Debug.Log(tvec.Step());
            //3
            //Debug.Log(tvec.Channels());




            //Debug.Log(tvec.At<double>(0, 0));
            //Debug.Log(tvec.At<double>(0, 1));
            //Debug.Log(tvec.At<double>(0, 2));

            /*
            Debug.Log(rvec.GetArray(0, 1));
            Debug.Log(rvec.GetArray(0, 2));
            */

        }

        cam_Texture.LoadImage(cam_frame.ImEncode());
    }
}
