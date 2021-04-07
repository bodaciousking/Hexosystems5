using UnityEngine;
using System.Collections;

namespace Assets.Scripts.AI
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class AITargetObject : MonoBehaviour
    {
        Targetting targetting;

        private void Start()
        {
            targetting = Targetting.instance;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "Main")
            {
                GameObject hextileObject = other.gameObject.transform.parent.gameObject;

                Hextile tileScript = hextileObject.GetComponent<Hextile>();

                Renderer hextileRenderer = hextileObject.transform.Find("Main").GetComponent<Renderer>();

                targetting.AddTileToAITargets(tileScript);
            }
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.name == "Main")
            {
                GameObject hextileObject = other.gameObject.transform.parent.gameObject;

                Hextile tileScript = hextileObject.GetComponent<Hextile>();

                Renderer hextileRenderer = hextileObject.transform.Find("Main").GetComponent<Renderer>();

                targetting.AddTileToAITargets(tileScript);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name == "Main")
            {
                GameObject hextileObject = other.gameObject.transform.parent.gameObject;

                Hextile tileScript = hextileObject.GetComponent<Hextile>();

                targetting.ResetColors();
                targetting.ClearTargets();

            }
        }
    }

}