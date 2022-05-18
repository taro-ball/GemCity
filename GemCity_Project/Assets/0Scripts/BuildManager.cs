using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    private TurretBlueprint turretToBuild;
    public GameObject buildEffect;
    public GameObject sellEffect;
    public GameObject buildPrompt;
    public NodeUI nodeUI;
    private Node selectedNode;
    private void Awake()
    {
        instance = this;
    }

    public bool CanBuild { get { return turretToBuild != null; } }
    public bool HasMoney { get { return PlayerStats.Money >= turretToBuild.cost; } }
    public void SelectTurretToBuild(TurretBlueprint turret)
    {
        GameObject effectInstance = (GameObject)Instantiate(buildPrompt, transform.position, transform.rotation);
        effectInstance.transform.SetParent(GameObject.Find("Canvas").transform,false);
        //effectInstance.transform.position.Set() = 500;
        //destroy effect after delay
        Destroy(effectInstance, 3f);
        turretToBuild = turret;
        DeselectNode();
    }

    public void SelectNode(Node node)
    {
        if (selectedNode == node)
        {
            DeselectNode();
            return;
        }
        selectedNode = node;
        turretToBuild = null;
        nodeUI.SetTarget(node);
    }
    public void DeselectNode()
    {
        selectedNode = null;
        nodeUI.Hide();
    }

    public TurretBlueprint GetTurretToBuild()
    {
        return turretToBuild;
    }
    //public void BuildTurretOn(Node node)
    //{
    //    if (PlayerStats.Money < turretToBuild.cost)
    //    {
    //        Debug.Log("Not enough!");
    //        return;
    //    }
    //    PlayerStats.Money -= turretToBuild.cost;
    //    GameObject turret = (GameObject)Instantiate(turretToBuild.prefab, node.GetBuildPosition(), Quaternion.identity);
    //    GameObject efct = (GameObject)Instantiate(buildEffect, node.GetBuildPosition(), Quaternion.identity);
    //    Destroy(efct, 3f);
    //    node.turret = turret;
    //    Debug.Log("Money left: " + PlayerStats.Money);
    //    //build tower
    //    //GameObject turretToBuild = buildManager.GetTurretToBuild();
    //    //turret = Instantiate(turretToBuild, transform.position + positionOffset, transform.rotation);
    //}

}
