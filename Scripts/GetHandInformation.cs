using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GetHandInformation : MonoBehaviour
{
    [SerializeField] private Transform TrackingSpace;
    private StreamWriter writer;

     private void Start() {
    string filePath=Path.Combine(Application.persistentDataPath, "HandInformation.txt"); 
    writer = new StreamWriter(filePath);   
    }

private void Update()
{// Nullチェックを追加
        if (TrackingSpace == null)
        {
            Debug.LogError("TrackingSpace is not assigned.");
            return;
        }
   // 右手
        // コントローラーの位置を取得
        Vector3 localPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
        Vector3 rpos = TrackingSpace.TransformPoint(localPos);

        // コントローラーの角度を取得
        Quaternion localRot = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
        Quaternion rrot = TrackingSpace.rotation * localRot;
        Debug.Log(rrot);

        // 左手
        // コントローラーの位置を取得
        Vector3 localPosLeft = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
        Vector3 lpos = TrackingSpace.TransformPoint(localPosLeft);

        // コントローラーの角度を取得
        Quaternion localRotLeft = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
        Quaternion lrot = TrackingSpace.rotation * localRotLeft;

     // データをテキストファイルに書き込む
        writer.WriteLine("Right Hand Position: " + rpos);
        writer.WriteLine("Right Hand Rotation: " + rrot.eulerAngles);
        writer.WriteLine("Left Hand Position: " + lpos);
        writer.WriteLine("Left Hand Rotation: " + lrot.eulerAngles);
}

 private void OnDestroy() {
      // writerがnullでないことを確認してからCloseメソッドを呼び出す
        if (writer != null)
        {
            writer.Close();  
        }
}
}
