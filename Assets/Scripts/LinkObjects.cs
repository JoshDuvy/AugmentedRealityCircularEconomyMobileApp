using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/* Link Objects Script - Attached to Vuforia AR Camera
 * 
 * This script searches for objects with the 'FoundObject' tag which is set when a Vuforia Recgonises an
 * image target. Dependant on the number of 'Found Objects' different visulisations will be displayed.
 * 
 * For More than two found objects, a text box will appear in the middle of all objects displaying
 * total waste of objects if used for a year. Landfills Models will also be created close to the text but in the direction
 * of each found object. Trucks will also be created on the found objects location.
 * 
 * For if one object is found then display pollution particles around the object, relative to the products
 * carbon factor.
 * 
 * For no found objects then hide certain visulisation elements
 * 
 */

public class LinkObjects : MonoBehaviour
{
    //Declare Variables for Script
    private GameObject[] FoundObjects;

    public GameObject SingularGameObject;
    public GameObject ComparisonText;
    public GameObject LandfillObject;
    public GameObject Landfilltruck;

    private string ComparisonTextString;
    
    private Vector3 FoundObjectMidPoint;
    private Vector3 LandfillPosition;
    private Vector3 FoundObjectPosition;

    private int PollutionLevelInt;
    private int ListLength = 0;
    private int x = 0;

    private float timeCheck = 0;
    private float LandfillScaleFloat;

    private decimal PackagingWeight;
    private decimal PackagingContents;
    private decimal WasteObjectTotal;
    private decimal PollutionLevel;
    private decimal WasteTotal;
    private decimal FoundObjectsWasteTotal;
    private decimal LandfillScale;

    // Update is called once per frame
    void Update()
    {
        //Set comparison check to check every half a second
        //Code from Unity Doc https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html

        timeCheck += Time.deltaTime;
        if (timeCheck >= 0.5f)
        {
            timeCheck = 0.0f;
            //End of Code from Unity Doc

            //reset list length
            ListLength = 0;

            //Search for objects that are in the camera viewfinder
            FoundObjects = GameObject.FindGameObjectsWithTag("FoundObject");

            //determine list length
            foreach (GameObject FoundObject in FoundObjects)
            {
                ListLength++;
            }

            //If there is more than two objects found, display comparison models
            if (ListLength >= 2)
            {



                //Set text to represent every item
                //Adapted from Text Mesh Pro documentation http://digitalnativestudios.com/textmeshpro/docs/ScriptReference/TextMeshPro-text.html
                ComparisonText.GetComponent<TextMeshPro>().text = ComparisonTextString;
                //End of Text Mesh pro code

                //reset foundObjectMidpoint and string
                FoundObjectPosition = new Vector3(0F, 0F, 0F);
                ComparisonTextString = "";

                /* calculate yearly waste for products if they consumers use 1litre
                 
                Search list and calculuate Total waste (Waste total variable) by dividing the packaging weight by the product's packaging contents
                this provides a value for the g of packaging per ml of contents
                multiply this by 1000 to make ml value into litre value
                multiple by days in years to make a total waste in a year value

                Add this total year waste value to a string which is displayed in the centre of all found objects

                */

                foreach (GameObject FoundObject in FoundObjects)
                {
                    //Create a total position for all values
                    FoundObjectPosition += FoundObject.GetComponent<Transform>().position;

                    //Find product characteristics
                    PackagingWeight = FoundObject.GetComponent<ProductCharacteristics>().PackagingWeightInt;
                    PackagingContents = FoundObject.GetComponent<ProductCharacteristics>().PackagingContentsInt;

                    //Calculation on products waste characteristics 
                    WasteTotal = decimal.Divide(PackagingWeight, PackagingContents);
                    WasteTotal = decimal.Multiply(WasteTotal, 1000.0m); // ml to litre
                    WasteTotal = decimal.Divide(WasteTotal, 1000.0m); // g to kg
                    WasteTotal = decimal.Multiply(WasteTotal, 365.0m); //Days in a year
                    WasteTotal = decimal.Round(WasteTotal);

                    //Set a products waste total in the objects characteristics
                    FoundObject.GetComponent<ProductCharacteristics>().WasteTotal = WasteTotal;

                    //create a total of all found objects yearly waste totals
                    FoundObjectsWasteTotal += WasteTotal;

                    //Display every items total waste produced
                    //code adpated from https://docs.unity3d.com/ScriptReference/Transform-parent.html
                    ComparisonTextString += $"{FoundObject.transform.parent.name} produces {WasteTotal} Kg of waste over the course of a year if you use 1 litre a day!{Environment.NewLine}{Environment.NewLine}";
                    //end of unity doc code
                    
                }

                //Set the midpoint between all found objects
                FoundObjectMidPoint = FoundObjectPosition / ListLength;

                //Find average objects waste total
                FoundObjectsWasteTotal /= ListLength;

                foreach (GameObject FoundObject in FoundObjects)
                {
                    //For every gameobject generate a landfill model and truck model
                    //Adapted code unity documentation https://docs.unity3d.com/ScriptReference/Object.Instantiate.html and https://docs.unity3d.com/ScriptReference/Quaternion.Euler.html
                    GameObject LandfillInstantiation;

                    //Calculate position of the landfill objects to be 80% (away from product) of the distance between the found object and the midpoint of all objects
                    LandfillPosition = new Vector3((((FoundObjectMidPoint.x - FoundObject.GetComponent<Transform>().position.x) * 0.8f) + FoundObject.GetComponent<Transform>().position.x), (((FoundObjectMidPoint.y - FoundObject.GetComponent<Transform>().position.y) * 0.2f) + FoundObject.GetComponent<Transform>().position.y), (((FoundObjectMidPoint.z - FoundObject.GetComponent<Transform>().position.z) * 0.2f) + FoundObject.GetComponent<Transform>().position.z));
                    LandfillInstantiation = Instantiate(LandfillObject, LandfillPosition, Quaternion.Euler(0, 0, 0));

                    //Scale the landfill model based on the relative size of the objects pollution compared to the average of all objects
                    LandfillScale = FoundObject.GetComponent<ProductCharacteristics>().WasteTotal / FoundObjectsWasteTotal;
                    LandfillScale *= 10.0m; //to aid in visulisation size
                    LandfillScaleFloat = decimal.ToSingle(LandfillScale);
                    LandfillInstantiation.GetComponent<Transform>().localScale = new Vector3(LandfillInstantiation.GetComponent<Transform>().localScale.x * LandfillScaleFloat, LandfillInstantiation.GetComponent<Transform>().localScale.y * LandfillScaleFloat, LandfillInstantiation.GetComponent<Transform>().localScale.z * LandfillScaleFloat);

                    //Create the landfill truck 
                    GameObject LandfillTruck;
                    LandfillTruck = Instantiate(Landfilltruck, FoundObject.GetComponent<Transform>().position, Quaternion.Euler(0, 0, 0));
                    LandfillTruck.GetComponent<LandfillTruckScript>().MovementPosition = LandfillPosition;

                    //End adapted segments of code from unity doc
                }

                //Display mesh renderer at the end to avoid misplaced models
                ComparisonText.GetComponent<MeshRenderer>().enabled = true;
                ComparisonText.GetComponent<Transform>().position = FoundObjectMidPoint;

            }
            //if lower than one object hide comparison models and show singular item model
            else if (ListLength == 1)
            {
                //Turn off comparison model to reduce visual noise 
                ComparisonText.GetComponent<MeshRenderer>().enabled = false;

                foreach (GameObject FoundObject in FoundObjects)
                {

                    /*calculate pollution level using database entries from: https://www.winnipeg.ca/finance/findata/matmgt/documents/2012/682-2012/682-2012_Appendix_H-WSTP_South_End_Plant_Process_Selection_Report/Appendix%207.pdf
                     and: https://www.ryedale.gov.uk/attachments/article/690/Different_plastic_polymer_types.pdf

                    When a singular object is in view of the camera (tagged 'Found'), read characteristics of the product displayed
                    within the ProductCharacteristics script which has been assigned values in inspector
                    Take the packaging contents in ml, divide it by the products packaging weight in g
                    multiply this by the kg carbon emission factors for particular material to produce PollutionLevelInt
                    this will produce a number below 1, as a result of rounding all numbers would be either 1 or 0 
                    to solve this multiplcation of the value by aids in the visualisation process

                    */

                    //Find object characteristics 
                    PackagingWeight = FoundObject.GetComponent<ProductCharacteristics>().PackagingWeightInt;
                    PackagingContents = FoundObject.GetComponent<ProductCharacteristics>().PackagingContentsInt;

                    //Fuction which calculates pollution is called with a materials carbon factor
                    if (FoundObject.GetComponent<ProductCharacteristics>().PackagingMaterial == "Glass")
                    {
                        PollutionCalculator(PackagingContents, PackagingWeight, 0.85m);
                    }
                    else if (FoundObject.GetComponent<ProductCharacteristics>().PackagingMaterial == "PET")
                    {
                        PollutionCalculator(PackagingContents, PackagingWeight, 5.44m);
                    }
                    else if (FoundObject.GetComponent<ProductCharacteristics>().PackagingMaterial == "HDPE")
                    {
                        PollutionCalculator(PackagingContents, PackagingWeight, 1.9m);
                    }
                    else if (FoundObject.GetComponent<ProductCharacteristics>().PackagingMaterial == "ALU")
                    {
                        PollutionCalculator(PackagingContents, PackagingWeight, 8.14m);
                    }

                    //Take pollution value for the product and create multiple small models to represent pollution around the object
                    //Adapted code unity documentation https://docs.unity3d.com/ScriptReference/Object.Instantiate.html
                    for (x = 0; x < PollutionLevelInt; x++)
                    {
                        //Create poluttion gameobjects within pollution 
                        Instantiate(SingularGameObject, new Vector3((FoundObject.GetComponent<Transform>().position.x + UnityEngine.Random.Range(-0.1f, 0.1f)), (FoundObject.GetComponent<Transform>().position.y + UnityEngine.Random.Range(-0.1f, 0.1f)), FoundObject.GetComponent<Transform>().position.z + UnityEngine.Random.Range(-0.1f, 0.1f)), Quaternion.identity);
                        //End code from unity doc
                    }
                }
            }

            //if lower hide all models
            else
            {
                ComparisonText.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    //Function which calculates pollution level for an object and returns the result
    private void PollutionCalculator(decimal PackagingContents, decimal PackagingWeight, decimal MaterialCarbonFactor)
    {
        if (PackagingWeight != 0) //if not null
        {
            //calculate collution level based on packaging weight and contents
            PollutionLevel = decimal.Divide(PackagingWeight, PackagingContents);
            PollutionLevel = decimal.Multiply(PollutionLevel, MaterialCarbonFactor);
            PollutionLevel = decimal.Multiply(PollutionLevel, 10.0m); // makes values greater than one for visualisation 
            PollutionLevel = decimal.Round(PollutionLevel); //Round for for instantiation
            PollutionLevelInt = decimal.ToInt32(PollutionLevel);
        }
        return;
    }
}
