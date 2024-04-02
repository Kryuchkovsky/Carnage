using System.IO;
using _GameLogic.Extensions;
using UnityEditor;
using UnityEngine;

namespace _Logic.Map
{
    public class LevelMapRenderer : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _levelMeshRenderer;
        [SerializeField] private Camera _mapCamera;
        [SerializeField] private Camera _markersCamera;

        private void Awake()
        {
            _mapCamera.enabled = false;
        }

        [ContextMenu("SetUpCameras")]
        public void SetUpCameras()
        {
            var levelBounds = _levelMeshRenderer.bounds;
            var levelPosition = _levelMeshRenderer.transform.position + levelBounds.center;
            var angle = Mathf.Deg2Rad * (_mapCamera.fieldOfView / 2);
            var ctg = 1 / Mathf.Tan(angle);
            var length = levelBounds.extents.z * ctg;
            var width = levelBounds.extents.x * ctg;
            var height = Mathf.Max(length, width);
            var cameraPosition = levelPosition + Vector3.up * height;

            _mapCamera.transform.position = cameraPosition;
            _markersCamera.transform.position = cameraPosition;

            var direction = levelPosition - cameraPosition;
            var rotation = Quaternion.LookRotation(direction);
            
            _mapCamera.transform.rotation = rotation;
            _markersCamera.transform.rotation = rotation;
        }

        [ContextMenu("BakeLevelMap")]
        public void BakeLevelMap()
        {
            SetUpCameras();
            _mapCamera.enabled = true;
            var mapTexture = ExtraMethods.GetTextureFromCamera(_mapCamera);
            var bytes = mapTexture.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + "/Resources/MapTexture.png", bytes);
            _mapCamera.enabled = false;
            AssetDatabase.Refresh();
        }
    }
}
