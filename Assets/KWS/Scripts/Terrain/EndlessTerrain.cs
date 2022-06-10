using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour
{
    public const float max_view_dist = 50;
    public Transform viewer;

    public static Vector2 viewer_position;
    int chunk_size;
    int chunk_visible_inView_dist;

    Dictionary<Vector2, TerrainChunk> terrainchunk_dic = new Dictionary<Vector2, TerrainChunk>();
    void Start()
    {
        chunk_size = 20;
        chunk_visible_inView_dist = Mathf.RoundToInt(max_view_dist / chunk_size);
    }
    void Update()
    {
        viewer_position = new Vector2(viewer.position.x, viewer.position.z);
        UpdateVisibleChunks();
    }
    
    private void UpdateVisibleChunks()
    {
        List<Vector2> keyList = new List<Vector2>(terrainchunk_dic.Keys);
        for (int i = 0; i < keyList.Count; i++)
        {
            Vector2 key = keyList[i];
            terrainchunk_dic[key].UpdateTerrainChunk();
        }

        int currentChunk_coordX = Mathf.RoundToInt(viewer_position.x / chunk_size);
        int currentChunk_coordY = Mathf.RoundToInt(viewer_position.y / chunk_size);

        for( int yOffset = -chunk_visible_inView_dist; yOffset <= chunk_visible_inView_dist; yOffset++)
        {
            for (int xOffset = -chunk_visible_inView_dist; xOffset <= chunk_visible_inView_dist; xOffset++)
            {
                Vector2 viewedChunkCoord = new Vector2(currentChunk_coordX + xOffset, currentChunk_coordY + yOffset);

                if(terrainchunk_dic.ContainsKey(viewedChunkCoord) == false)
                {
                    terrainchunk_dic.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, chunk_size));
                }
            }
        }
    }

    private void OnDestroy()
    {
        Debug.Log("Terrain Destroy");
    }
    public class TerrainChunk
    {
        GameObject mesh_object;
        Vector2 position;
        Bounds bounds;

        Vector3 position_v3;
        Vector3 local_scale;

        bool before_visible = false;
        public TerrainChunk(Vector2 coord, int size)
        {
            position = coord * size;
            bounds = new Bounds(position, Vector2.one * size);

            position_v3 = new Vector3(position.x, 0, position.y);

            //if (mesh_object == null)
            //    return;
            mesh_object = ObjectPooling.instance.GetPlane();
            if (mesh_object == null)
                return;
            mesh_object.transform.position = position_v3;

            local_scale = Vector3.one * size / 10f;
            mesh_object.transform.localScale = local_scale;

            before_visible = true;
        }
        public void UpdateTerrainChunk()
        {
            float viewerdist_fromNearest_edge = Mathf.Sqrt(bounds.SqrDistance(viewer_position));
            bool visible = viewerdist_fromNearest_edge <= max_view_dist;

            if (before_visible != visible)
            {
                before_visible = visible;
                SetVisible(visible);
            }
        }
        
        public void SetVisible(bool visible)
        {
            if (visible == false)
            {
                ObjectPooling.instance.ReturnPlane(mesh_object);
                mesh_object = null;
            }
            else
            {
                mesh_object = ObjectPooling.instance.GetPlane();
                mesh_object.transform.position = position_v3;
                mesh_object.transform.localScale = local_scale;
            }
        }
    }
}
