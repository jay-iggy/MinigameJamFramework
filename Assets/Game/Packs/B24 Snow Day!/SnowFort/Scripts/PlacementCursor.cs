using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnowDay.Snowfort
{
    public class PlacementCursor : MonoBehaviour
    {
        public static readonly Color[] cursorColors = { new Color(0, 255, 200), new Color(200, 255, 0), 
            new Color(255, 150, 0), new Color(255, 0, 200) };

        bool enabledState = false;

        public bool rightTeam = false;

        public Color valid;
        public Color invalid;

        int buildingIndex;
        public List<BuildingObject> objects;
        public List<int> counts;

        public GameObject preview;
        Vector3 blockPosition;
        Vector3 overridePosition;
        bool overridePreview;

        int leftTeamMask;
        int rightTeamMask;

        PlayerController playerController;

        public void MoveCursor(Vector2 movement)
        {
            if (!enabledState) return;

            Vector3 pos = transform.position + (Vector3) movement;
            if (rightTeam)
            {
                pos.x = Mathf.Min(pos.x, MapController.active.boundsRbr.x - 0.01f);
                pos.x = Mathf.Max(pos.x, MapController.active.boundsRtl.x);
                pos.y = Mathf.Min(pos.y, MapController.active.boundsRtl.y - 0.01f);
                pos.y = Mathf.Max(pos.y, MapController.active.boundsRbr.y);
            }
            else
            {
                pos.x = Mathf.Min(pos.x, MapController.active.boundsLbr.x - 0.01f);
                pos.x = Mathf.Max(pos.x, MapController.active.boundsLtl.x);
                pos.y = Mathf.Min(pos.y, MapController.active.boundsLtl.y - 0.01f);
                pos.y = Mathf.Max(pos.y, MapController.active.boundsLbr.y);
            }
            transform.position = pos;

            if (preview == null) return;

            if (blockPosition != RoundToUnit(transform.position))
            {
                blockPosition = RoundToUnit(transform.position);
                if (CheckOverride())
                {
                    Vector3 previewPos = RoundToUnit(Physics2D.OverlapArea(VecFloor(transform.position), VecFloor(transform.position) + new Vector3(1, 1),
                        MapController.active.placementCheckMask).transform.position);

                    if (previewPos == overridePosition && overridePreview)
                        return;

                    Destroy(preview);
                    preview = Instantiate(GetOverrideData().preview, previewPos, Quaternion.identity);
                    if (rightTeam && !GetOverrideData().disablePreviewFlip) preview.transform.localScale = new Vector3(-1, 1, 1);
                    overridePosition = previewPos;
                    overridePreview = true;
                }
                else if (overridePreview)
                {
                    if (preview != null)
                        Destroy(preview);

                    overridePreview = false;
                    preview = Instantiate(objects[buildingIndex].preview, Vector3.zero, Quaternion.identity);
                    if (rightTeam) preview.transform.localScale = new Vector3(-1, 1, 1);
                }

            }

            if (overridePreview)
                preview.transform.position = overridePosition;
            else
                preview.transform.position = RoundToUnit(transform.position) + new Vector3(0, 0, -0.5f);
        }

        public void Scroll(bool up)
        {
            if (!enabledState || objects.Count == 0) return;

            buildingIndex += up ? 1 : -1;
            buildingIndex %= objects.Count;
            if (buildingIndex < 0) buildingIndex += objects.Count;

            if (preview != null)
                Destroy(preview);

            if (CheckOverride())
            {
                Vector3 previewPos = RoundToUnit(Physics2D.OverlapArea(VecFloor(transform.position), 
                    VecFloor(transform.position) + new Vector3(1, 1), MapController.active.placementCheckMask).transform.position);
                preview = Instantiate(GetOverrideData().preview, previewPos, Quaternion.identity);
                if (rightTeam && !GetOverrideData().disablePreviewFlip) preview.transform.localScale = new Vector3(-1, 1, 1);
                overridePosition = previewPos;
                overridePreview = true;
            }
            else
            {
                preview = Instantiate(objects[buildingIndex].preview, RoundToUnit(transform.position), Quaternion.identity);
                if (rightTeam) preview.transform.localScale = new Vector3(-1, 1, 1);
                overridePreview = false;
            }
        }

        public void Place()
        {
            if (!enabledState) return;

            bool i = CheckValidBuildPlacement();
            if (!i) return;

            if (!CheckOverride())
            {
                GameObject obj = Instantiate(objects[buildingIndex].gameobject, RoundToUnit(transform.position), Quaternion.identity);
                if (rightTeam) obj.transform.localScale = new Vector3(-1, 1, 1);

                TeamIgnore t = obj.GetComponent<TeamIgnore>();
                if (t != null)
                    obj.layer = rightTeam ? rightTeamMask : leftTeamMask;

                Controllable c = obj.GetComponent<Controllable>();
                if (c != null)
                {
                    playerController.fort.controllables.Add(c);
                    c.SetTeam(rightTeam);
                }
            }
            else
            {
                Physics2D.OverlapArea(VecFloor(transform.position), VecFloor(transform.position) + new Vector3(1, 1),
                MapController.active.placementCheckMask).transform.parent.GetComponent<BuildingOverride>().BuildOverride(objects[buildingIndex]);
            }

            counts[buildingIndex]--;
            if (counts[buildingIndex] == 0)
            {
                objects.RemoveAt(buildingIndex);
                counts.RemoveAt(buildingIndex);
                if (objects.Count == 0)
                    playerController.SetState(PlayerController.PlayerState.OBSERVER);
                else
                    Scroll(true);
            }
        }

        public void AddObject(BuildingObject obj, int count = 1) {
            int i = objects.FindIndex(a => a == obj);
            if (i == -1)
            {
                objects.Add(obj);
                counts.Add(count);
            }
            else
            {
                counts[i] += count;
            }

        }

        public void SetEnabledState(bool enable)
        {
            enabledState = enable;

            if (preview != null)
                preview.gameObject.SetActive(enable);

            if (playerController.playerIndex != -1)
                GetComponent<SpriteRenderer>().color = cursorColors[playerController.playerIndex];
        
            GetComponent<SpriteRenderer>().enabled = enable;

            Scroll(true);
        }

        void Update()
        {
            if (!enabledState) return;

            if (preview != null)
            {
                preview.GetComponent<PreviewController>().SetValid(CheckValidBuildPlacement());
            }
        }

        bool CheckValidBuildPlacement()
        {
            if (!enabledState) return false;

            Collider2D col = Physics2D.OverlapArea(VecFloor(transform.position), VecFloor(transform.position) + new Vector3(1, 1),
                MapController.active.placementCheckMask);
            if (col != null)
            {
                BuildingOverride b = col.transform.parent.GetComponent<BuildingOverride>();
                if (b != null) return b.CanOverride(objects[buildingIndex]);
                return false;
            }
            col = Physics2D.OverlapArea(VecFloor(transform.position),
                VecFloor(transform.position) + (Vector3Int)objects[buildingIndex].size, MapController.active.placementCheckMask);
            if (col != null) return false;

            if (rightTeam)
            {
                return !(VecFloor(transform.position).x + objects[buildingIndex].size.x > MapController.active.boundsRbr.x
                    || VecFloor(transform.position).y + objects[buildingIndex].size.y > MapController.active.boundsRtl.y);
            }

            return !(VecFloor(transform.position).x + objects[buildingIndex].size.x > MapController.active.boundsLbr.x
                || VecFloor(transform.position).y + objects[buildingIndex].size.y > MapController.active.boundsLtl.y);
        }

        bool CheckOverride()
        {
            Collider2D col = Physics2D.OverlapArea(VecFloor(transform.position), VecFloor(transform.position) + new Vector3(1, 1),
                MapController.active.placementCheckMask);
            if (col == null) return false;
            BuildingOverride b = col.transform.parent.GetComponent<BuildingOverride>();
            if (b == null) return false;
            return b.OverrideExists(objects[buildingIndex]);
        }

        BuildingOverrideData GetOverrideData()
        {
            return Physics2D.OverlapArea(VecFloor(transform.position), VecFloor(transform.position) + new Vector3(1, 1),
                MapController.active.placementCheckMask).transform.parent.GetComponent<BuildingOverride>().OverrideData(objects[buildingIndex]);
        }

        Vector3 RoundToUnit(Vector3 position)
        {
            return new Vector3(Mathf.Floor(position.x) + 0.5f, Mathf.Floor(position.y) + 0.5f);
        }

        Vector3 VecFloor(Vector3 vec)
        {
            return new Vector3(Mathf.Floor(vec.x), Mathf.Floor(vec.y));
        }

        void Start()
        {            
            playerController = transform.parent.GetComponent<PlayerController>();

            GameManager g = FindObjectOfType<GameManager>();
            leftTeamMask = g.leftTeam;
            rightTeamMask = g.rightTeam;
        }
    }
}
