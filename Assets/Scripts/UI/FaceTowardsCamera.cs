using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaceTowardsCamera : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Camera _camera;
    // Start is called before the first frame update
    private void Start()
    {
        if (_canvas == null)
        {
            Debug.LogError("FaceTowardsCamera: Canvas is null!");
        }
        if (_camera == null)
        {
            _camera = Camera.main;
        }
        if (_camera == null)
        {
            Debug.LogError("FaceTowardsCamera: Camera is null!");
        }
    }
    // Update is called once per frame
    private void Update()
    {
        _canvas.transform.LookAt(_camera.transform);
    }
}
