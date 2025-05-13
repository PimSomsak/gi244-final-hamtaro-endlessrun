using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;

public class ObstacleButtonSkill : MonoBehaviour
{
    public InputAction skill1Action;

    public GameObject obstaclePrefab;
    public GameObject obstaclePrefab2;
    public GameObject obstaclePrefab3;
    public Vector3[] spawnPos;
    public float coolDown = 2;
    public bool isCoolingDown ;
    public Image imgLoad;

    public float coolDown2 = 2;
    public bool isCoolingDown2;
    public Image imgLoad2;

    public float coolDown3 = 2;
    public bool isCoolingDown3;
    public Image imgLoad3;

    public GraphicRaycaster uiRaycaster;
    public EventSystem eventSystem;

    public void Start()
    {
        skill1Action = InputSystem.actions.FindAction("Skill1");

        imgLoad.fillAmount = 0;
        imgLoad2.fillAmount = 0;
        imgLoad3.fillAmount = 0;
    }
   
    private void Update()
    {
        if (skill1Action.triggered)
        {
            if (!isCoolingDown && IsPointerOverImage(imgLoad))
            {
                TriggerSkill(ref isCoolingDown, imgLoad, obstaclePrefab);
            }
            else if (!isCoolingDown2 && IsPointerOverImage(imgLoad2))
            {
                TriggerSkill(ref isCoolingDown2, imgLoad2, obstaclePrefab2);
            }
            else if (!isCoolingDown3 && IsPointerOverImage(imgLoad3))
            {
                TriggerSkill(ref isCoolingDown3, imgLoad3, obstaclePrefab3);
            }
        }

        UpdateCooldown(ref isCoolingDown, imgLoad, coolDown);
        UpdateCooldown(ref isCoolingDown2, imgLoad2, coolDown2);
        UpdateCooldown(ref isCoolingDown3, imgLoad3, coolDown3);      
    }

    private void TriggerSkill(ref bool isCooling, Image img, GameObject prefab)
    {
        isCooling = true;
        img.fillAmount = 1f;

        Instantiate(
            prefab,
            spawnPos[Random.Range(0, spawnPos.Length)],
            prefab.transform.rotation
        );
    }

    private void UpdateCooldown(ref bool isCooling, Image img, float cd)
    {
        if (isCooling)
        {
            img.fillAmount -= Time.deltaTime / cd;
            if (img.fillAmount <= 0f)
            {
                img.fillAmount = 0f;
                isCooling = false;
            }
        }
    }

    private bool IsPointerOverImage(Image targetImage)
    {
        PointerEventData pointerData = new PointerEventData(eventSystem);
        pointerData.position = Mouse.current.position.ReadValue();

        List<RaycastResult> results = new List<RaycastResult>();
        uiRaycaster.Raycast(pointerData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject == targetImage.gameObject)
            {
                return true;
            }
        }
        return false;
    }

}
