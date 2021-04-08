using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    public List<GameObject> tiles;

    public GameObject fog;
    public GameObject temp; 

    public GameObject hextile;
    public GameObject planetSphere;

    public int numPlayers;
    public float planetSize = 40;

    public List<Material> mats = new List<Material>();


    void Start()
    {
        for (int i = 0; i <= numPlayers -1; i++)
        {
            string holderName = "Player " + i.ToString() + " Map";
            if (transform.Find(holderName))
            {
                DestroyImmediate(transform.Find(holderName).gameObject);
            }
            Transform mapHolder = new GameObject(holderName).transform;
            mapHolder.parent = transform;

            List<Hextile> hextileList = new List<Hextile>(); // revert back when done  

            int numRows = 15;

            GameObject planetObject = Instantiate(planetSphere, mapHolder.transform.position, Quaternion.identity);
            planetObject.transform.position += new Vector3((numRows - 1) / 2, -(planetSize/2) +1.5f, -1.5f);
            planetObject.transform.localScale *= planetSize;
            Color planetColor = Color.white;//
            planetObject.GetComponent<Renderer>().material.color = planetColor;
            planetObject.transform.parent = mapHolder;
            planetObject.transform.parent = mapHolder;
            HexoPlanet planet = planetObject.gameObject.GetComponent<HexoPlanet>();
            planet.owningPlayerID = i;
            planet.GetComponent<Renderer>().material = mats[i];

            for (int k = 1; k <= numRows; k++)
            {
                int rowLength = DetermineRowLength(k, numRows);
                float rowCenter = (rowLength / 2);

                string rowName = "Row Holder " + k.ToString();
                Transform rowHolder = new GameObject(rowName).transform;
                rowHolder.parent = mapHolder;

                for (int j = 0; j < rowLength; j++)
                {
                    GameObject newTile = Instantiate(hextile, new Vector3(k, 0, j - rowCenter), Quaternion.identity, rowHolder);
                    var euler = newTile.transform.eulerAngles; //Rotate the city randomly so they look a little random.
                    euler.y = -150;
                    newTile.transform.eulerAngles = euler;
                    tiles.Add(newTile);
                    
                    GameObject floor = newTile.transform.Find("Main").gameObject;
                    floor.GetComponent<Renderer>().material.color = Color.white;
                    floor.GetComponent<FloorGfx>().myColor = planetColor;

                    Transform cityObject = newTile.transform.Find("City");
                    var euler2 = newTile.transform.Find("City").eulerAngles; //Rotate the city randomly so they look a little random.
                    euler2.y = Random.Range(0, 360);
                    cityObject.eulerAngles = euler2;
                    cityObject.localScale += new Vector3(0, Random.Range(0f, 2f), 0);
                    cityObject.gameObject.SetActive(false);

                    Hextile currentTileScript = newTile.GetComponent<Hextile>();
                    currentTileScript.tileLocation = new Vector2(k, j);
                    currentTileScript.owningPlayerID = i;
                    hextileList.Add(currentTileScript);
                    currentTileScript.myRowLength = rowLength;
                }


                if (k % 2 == 0) // Here we're checking if this row is an odd number, and if it is, shifting its z position by -0.5
                {
                }
                else
                    rowHolder.position += new Vector3(0, 0, -0.5f);
            }
            planet.hextileList = hextileList;
            //Offset the entire player's grid based on the number of players
            mapHolder.transform.position = mapHolder.parent.position;
            if(i == 0)
            {
                mapHolder.Rotate(-3F, -1.5f, -3.4F);
                planet.transform.position = new Vector3(10, -20, -12);
            }
            if (i == 1)
            {
                mapHolder.transform.position += new Vector3(-10, 0, 35);
                mapHolder.Rotate(-8F, 4.2F, 0);
                planet.transform.position += new Vector3(-7, 0, 10);
            }
        }
        //FogGen();
    }

    public int DetermineRowLength (int currentRow, int numRows)
    {
        int rowLength = 0;
        if (currentRow == 1 || currentRow == numRows) 
            rowLength = 3;
        else if (currentRow == 2 || currentRow == numRows - 1)
            rowLength = 6;
        else if (currentRow == 3 || currentRow == numRows - 2)
            rowLength = 9;
        else if (currentRow == 4 || currentRow == numRows - 3)
            rowLength = 10;
        else if (currentRow == 5 || currentRow == numRows - 4)
            rowLength = 11;
        else if (currentRow == 6 || currentRow == numRows - 5)
            rowLength = 12;
        else if (currentRow == 7 || currentRow == numRows - 6)
            rowLength = 13;
        else if (currentRow == 8)
            rowLength = 12;

        if (rowLength == 0)
        {
            Debug.Log("Couldnt determine accurate rowLength!");
        }
        return rowLength;
       
    }

    // Update is called once per frame
    void Update()
    {
        

    }

     public void FogGen()
    {
       Vector2 tempVec; 

       for(int c = 0; c <= tiles.Count; c++)
        {
            temp = tiles[c];
            tempVec = temp.GetComponent<Hextile>().tileLocation;

            if (temp.GetComponent<Hextile>().visible == false)
            {

                Instantiate(fog, new Vector3(temp.transform.position.x, temp.transform.position.y+5, temp.transform.position.z), Quaternion.identity);
            }
        }
    }



}
