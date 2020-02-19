using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class UIHandler : MonoBehaviour
{
    

    private IslandSingle Owner = null;
    public Button leftButton, rightButton, bottomButton;
    private Text leftButtonText, rightButtonText, bottomButtonText;
    private GameObject [] tower_prefabs;

    public struct uiParameter {
        public GameObject tower1; //left
        public GameObject tower2; //right
        public bool sellTower;
    }

    public uiParameter actualUI;

    void Start()
    {
        leftButtonText = leftButton.transform.GetChild(0).GetComponent<Text>();
        rightButtonText = rightButton.transform.GetChild(0).GetComponent<Text>();
        bottomButtonText = bottomButton.transform.GetChild(0).GetComponent<Text>();
        tower_prefabs = GameManager.instance.tower_prefabs;
        actualUI.sellTower = false;

        actualUI.tower1 = tower_prefabs[0];
        actualUI.tower2 = tower_prefabs[1];
        hide();
    }

    public void askOwnerShip(GameObject caller){
        Debug.Log("askOwnerShip");
        //if someone ask for ownership while an other already own it, 
        //the new caller is the new owner.
        //We announce to the previous owner that he dont own us anymore
        if(Owner != null){
            Owner.setOwnerShip(false);
        }
        else {
            //if that's the first time that it is called, we display the true UI
            show();
        }
        Owner = caller.GetComponent<IslandSingle>();
        Owner.setOwnerShip(true);
        updateUI();
    }

    public void dismissOwnerShip(GameObject caller){
        Debug.Log("dismissOwnerShip");
        IslandSingle callingComponent = caller.GetComponent<IslandSingle>();
        //If the one asking for dismis ownership is the actual owner, let's go
        if(Owner == callingComponent){
            Owner.setOwnerShip(false);
            Owner = null;
            //We hide the UI
            hide();
        }
        else {
            //If a component asking for dismissing ownership, 
            //then it is a Bug and he shouldnt believe to be the owner
            Debug.Log("You are not the owner and cant ask for dismissingOwnership");
            callingComponent.setOwnerShip(false);
        }
    }
    
    public void updateUI(){
        if(Owner == null)
        return;//we cant updateUi if we dont have any person to desplay

        GameObject tower = Owner.askTower();
        rightButton.interactable = false;
        leftButton.interactable = false;
                
        CannonBehavior t1CBComponent = actualUI.tower1.GetComponent<CannonBehavior>();
        CannonBehavior t2CBComponent = actualUI.tower2.GetComponent<CannonBehavior>();
        //analysing situation and updating model
        if(tower != null){
            
            GameObject towerUpgrade = tower.GetComponent<CannonBehavior>().upgradeTo;
            GameObject towerprefab = PrefabUtility.GetCorrespondingObjectFromSource(tower);
            //S'il peut etre upgrade, on permet l'interaction
            if(actualUI.tower1 == towerprefab || t1CBComponent.upgradeTo == towerprefab)
            {
                actualUI.tower1 = towerUpgrade;
                t1CBComponent = actualUI.tower1.GetComponent<CannonBehavior>();
                if (t1CBComponent.upgradeTo != null && t1CBComponent.upgradeTo.GetComponent<CannonBehavior>().cost <= GameManager.instance.money)
                    leftButton.interactable = true;
            }
               
            if(actualUI.tower2 == towerprefab || t2CBComponent.upgradeTo == towerprefab)
            {
                actualUI.tower2 = towerUpgrade;
                t2CBComponent = actualUI.tower2.GetComponent<CannonBehavior>();
                if (t2CBComponent.upgradeTo != null &&  t2CBComponent.upgradeTo.GetComponent<CannonBehavior>().cost <= GameManager.instance.money)
                    rightButton.interactable = true;
            }
                
        }
        else {
            actualUI.tower1 = tower_prefabs[0];
            actualUI.tower2 = tower_prefabs[1];

            //si l'on a les sous et qu'il y a une upgrade
            
            if (actualUI.tower1.GetComponent<CannonBehavior>().cost <= GameManager.instance.money)
                leftButton.interactable = true;
            if (actualUI.tower2.GetComponent<CannonBehavior>().cost <= GameManager.instance.money)
                rightButton.interactable = true;
        }


        //applying model to view    
        leftButtonText.text = actualUI.tower1.name + "\n" + t1CBComponent.fireRate + " Fire/Seconds\n" + t1CBComponent.range + "m of Range\n" + t1CBComponent.DamagePerFire + " of Damage\ncost = " + t1CBComponent.cost + "$";
        rightButtonText.text = actualUI.tower1.name + "\n" + t2CBComponent.fireRate + " Fire/Seconds\n" + t2CBComponent.range + "m of Range\n" + t2CBComponent.DamagePerFire + " of Damage\ncost = " + t2CBComponent.cost + "$";
        bottomButtonText.text = ( tower != null ) ? "sell tourelle (+" + ( tower.GetComponent<CannonBehavior>().cost *  GameManager.instance.sellingFactor ) + "$)" : "Sell island (+" + GameManager.instance.islandPrice + "$)";

    }

    public void rightButtonClick(){
        createTurret(actualUI.tower2);
    }

    public void leftButtonClick(){
        createTurret(actualUI.tower1);
    }

    private void createTurret(GameObject towerToBuild){
        if (towerToBuild.GetComponent<CannonBehavior>().cost > GameManager.instance.money){
            Debug.Log("Not Enough Money");
            return;
        }
        Debug.Log("leftButtonClick");

        GameObject tower = Owner.askTower();
        if(tower)
            Destroy(tower);
        
        Instantiate(towerToBuild,Owner.transform.parent);
        GameManager.instance.money -= towerToBuild.GetComponent<CannonBehavior>().cost;
        dismissOwnerShip(Owner.gameObject);
    }

    public void bottomButtonClick(){
        Debug.Log("bottomButtonClick");
        GameObject tower = Owner.askTower();
        if(tower){
            GameManager.instance.money += tower.GetComponent<CannonBehavior>().cost *  GameManager.instance.sellingFactor;
            Destroy(tower);
        }
        else {
            GameManager.instance.money += GameManager.instance.islandPrice;
            Destroy(Owner.transform.parent);
        }
        dismissOwnerShip(Owner.gameObject);

    }

    private void show(){
        foreach(Transform child in transform){
            child.gameObject.SetActive(true);
        }
    }
    private void hide(){
        foreach(Transform child in transform){
            child.gameObject.SetActive(false);
        }
    }
}
