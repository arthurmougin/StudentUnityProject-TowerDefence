using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class UIHandler : MonoBehaviour
{
    

    private IslandSingle Owner = null;
    public Button leftButton, rightButton, bottomButton, middleButton;
    private Text leftButtonText, rightButtonText, bottomButtonText, middleButtonText;
    private GameObject [] tower_prefabs;
    public bool mainBool = false;

    public struct uiParameter {
        public GameObject tower1; //left
        public GameObject tower2; //right
    }

    public uiParameter actualUI;

    public 

    void Start()
    {
        leftButtonText = leftButton.transform.GetChild(0).GetComponent<Text>();
        rightButtonText = rightButton.transform.GetChild(0).GetComponent<Text>();
        bottomButtonText = bottomButton.transform.GetChild(0).GetComponent<Text>();
        middleButtonText = middleButton.transform.GetChild(0).GetComponent<Text>();
        tower_prefabs = GameManager.instance.tower_prefabs;

        actualUI.tower1 = tower_prefabs[0];
        actualUI.tower2 = tower_prefabs[1];
        hide();
    }

    // Update is called once per frame
    void Update()
    {
        /*if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("click");
            if (!EventSystem.current.IsPointerOverGameObject())
                OnClickSomewhereElse();
        }*/

    }

    public void askOwnerShip(GameObject caller){
        //Debug.Log("askOwnerShip");
        //if someone ask for ownership while an other already own it, 
        //the new caller is the new owner.
        //We announce to the previous owner that he dont own us anymore
        mainBool = true;
        if (Owner != null){
            Owner.setOwnerShip(false);
        }
        
        show();
        Owner = caller.GetComponent<IslandSingle>();
        Owner.setOwnerShip(true);
        updateUI(); 
        
    }

    public void dismissOwnerShip(GameObject caller){
        //Debug.Log("dismissOwnerShip");
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
        
        if(tower != null){

            //we hide every button
            rightButton.gameObject.SetActive(false);
            leftButton.gameObject.SetActive(false);
            
            
           //we check if it have an upgrade
            //Debug.Log("analysing tourelle");
            GameObject towerUpgrade = tower.GetComponent<CannonBehavior>().upgradeTo;
            
            //if it does
            if(towerUpgrade != null){
                //we show the upgrade button and update it's content
                middleButton.gameObject.SetActive(true);
                CannonBehavior upgradeComponent = towerUpgrade.GetComponent<CannonBehavior>();
                middleButtonText.text = towerUpgrade.name + "\n" + upgradeComponent.fireRate + " Fire/Seconds\n" + upgradeComponent.range + "m of Range\n" + upgradeComponent.DamagePerFire + " of Damage\ncost = " + upgradeComponent.cost + "$";
                
                //we test if the player can afford it and turn the button on or off depending on the result
                middleButton.interactable = (towerUpgrade.GetComponent<CannonBehavior>().cost <= GameManager.instance.money)? true : false;
            }
            else {
                //if it dont have any upgrade
                middleButton.gameObject.SetActive(false);
            }
             
            //Debug.Log("EndAnalysing");
        }
        else {
            //we update the good ui
            rightButton.gameObject.SetActive(true);
            leftButton.gameObject.SetActive(true);
            middleButton.gameObject.SetActive(false);

            CannonBehavior t1CBComponent = actualUI.tower1.GetComponent<CannonBehavior>();
            CannonBehavior t2CBComponent = actualUI.tower2.GetComponent<CannonBehavior>();

            actualUI.tower1 = tower_prefabs[0];
            actualUI.tower2 = tower_prefabs[1];

            // we set on or off the buttons dependanding on their aviability
            leftButton.interactable = (actualUI.tower1.GetComponent<CannonBehavior>().cost <= GameManager.instance.money)? true : false;
            rightButton.interactable = (actualUI.tower2.GetComponent<CannonBehavior>().cost <= GameManager.instance.money)? true : false;

            //updating data
            leftButtonText.text = actualUI.tower1.name + "\n" + t1CBComponent.fireRate + " Fire/Seconds\n" + t1CBComponent.range + "m of Range\n" + t1CBComponent.DamagePerFire + " of Damage\ncost = " + t1CBComponent.cost + "$";
            rightButtonText.text = actualUI.tower1.name + "\n" + t2CBComponent.fireRate + " Fire/Seconds\n" + t2CBComponent.range + "m of Range\n" + t2CBComponent.DamagePerFire + " of Damage\ncost = " + t2CBComponent.cost + "$";
        }

        bottomButtonText.text = ( tower != null ) ? "sell tourelle (+" + ( tower.GetComponent<CannonBehavior>().cost *  GameManager.instance.sellingFactor ) + "$)" : "Sell island (+" + GameManager.instance.islandPrice + "$)";
    }

    public void rightButtonClick(){
        createTurret(actualUI.tower2);
    }

    public void leftButtonClick(){
        createTurret(actualUI.tower1);
    }

    public void middleButtonClick(){
        createTurret(Owner.askTower().GetComponent<CannonBehavior>().upgradeTo);
        //Debug.Log("middleClick");
    }

    private void createTurret(GameObject towerToBuild){
        if (towerToBuild.GetComponent<CannonBehavior>().cost > GameManager.instance.money){
            Debug.Log("Not Enough Money");
            return;
        }

        GameObject tower = Owner.askTower();
        if(tower)
            Destroy(tower);
        
        Instantiate(towerToBuild,Owner.transform.parent);
        GameManager.instance.money -= towerToBuild.GetComponent<CannonBehavior>().cost;
        dismissOwnerShip(Owner.gameObject);
    }

    public void bottomButtonClick(){
        //Debug.Log("bottomButtonClick");
        GameObject tower = Owner.askTower();
        if(tower){
            GameManager.instance.money += tower.GetComponent<CannonBehavior>().cost *  GameManager.instance.sellingFactor;
            Destroy(tower);
        }
        else {
            GameManager.instance.money += GameManager.instance.islandPrice;
            Destroy(Owner.transform.parent.gameObject);
        }
        dismissOwnerShip(Owner.gameObject);

    }

    private void show(){
        foreach(Transform child in transform){
            child.gameObject.SetActive(true);
        }
        Debug.Log("show");
    }

    private void hide(){
        foreach(Transform child in transform){
            child.gameObject.SetActive(false);
        }
        Debug.Log("hide");
    }

    public void OnClickSomewhereElse()
    {
        Debug.Log("zergqegegerg");
        if (Owner == null)
            return;

        Owner.setOwnerShip(false);
        Owner = null;
        hide();

    }

     public void clickSurBackground()
    {

        if (mainBool)
        {
            mainBool = false;
        }
        else
        {
            Debug.Log("click background");
            hide();
            Owner.setOwnerShip(false);
            Owner = null;
            
        }
            
    }


}
