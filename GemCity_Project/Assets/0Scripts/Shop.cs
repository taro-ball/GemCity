using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public TurretBlueprint ArcherTower;
    public TurretBlueprint CanonTower;
    public TurretBlueprint MagicTower;
    BuildManager buildManager;
    // Start is called before the first frame update
    private void Start()
    {
        buildManager = BuildManager.instance;
    }
    public void SelectArcherTurret()
    {
        Debug.Log("Archer T Bought!");
        //buildManager.SetTowerToBuild(buildManager.archerTurretPrefab);
        buildManager.SelectTurretToBuild(ArcherTower);
    }
    public void SelectMagicTurret()
    {
        Debug.Log("Magic T Bought!");
        //buildManager.SetTowerToBuild(buildManager.MagicTurretPrefab);
        buildManager.SelectTurretToBuild(MagicTower);
    }
    public void SelectCanonTurret()
    {
        Debug.Log("Canon T Bought!");
        //buildManager.SetTowerToBuild(buildManager.canonTurretPrefab);
        buildManager.SelectTurretToBuild(CanonTower);
    }

}
