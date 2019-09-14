using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableSpawner : MonoBehaviour
{
    #region SERIALIZED_VARIABLES
    [SerializeField]
    private GameObject cablePrefab, parentObject;

    [SerializeField]
    [Range(1f, 1000f)]
    [Tooltip("Total length of cable with all of the cable parts")]
    private int cableLength = 1;



    [SerializeField]
    private float rotationLimit = 120f;

    [SerializeField]
    [Tooltip("distance between cable parts, which are capsules")]
    private float distanceBetweenCablePartPrefabs = .21f;

    [SerializeField]
    private bool resetParts;
    [SerializeField]
    private bool spawnCableParts;
    [SerializeField]
    private bool snapFirst;
    [SerializeField]
    private bool snapLast;

    #endregion

    //timestep for VR: 1/90 fixed timestep
    //timestep for physics: .005 fixed timestep, .02 max allowed timestep
    private void Start()
    {
        spawnCableParts = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if(resetParts)
        {
            foreach(GameObject tmpCablePart in GameObject.FindGameObjectsWithTag("Cable"))
            {
                Destroy(tmpCablePart);
            }

            resetParts = false; //will only run one time for each time you check it in the inspector
        }

        if(spawnCableParts)
        {
            SpawnCableParts();
            spawnCableParts = false;
        }
    }

    public void SpawnCableParts()
    {
        int numberOfSpawnedParts = (int)(cableLength / distanceBetweenCablePartPrefabs);

        for(int i = 0; i < numberOfSpawnedParts; i++)
        {
            GameObject spawnedGameObject;

            spawnedGameObject = Instantiate(cablePrefab, new Vector3(transform.position.x, 
                transform.position.y + distanceBetweenCablePartPrefabs * (i+1), transform.position.z), 
                Quaternion.identity, parentObject.transform);

            //180 is total allowed rotation around the x axis
            spawnedGameObject.transform.eulerAngles = new Vector3(rotationLimit, 0, 0);

            //name the spawned Cable Prefab after whatever child number it is 
            //under the parent, i.e. 1, 2, 3, etc.
            spawnedGameObject.name = parentObject.transform.childCount.ToString();

            if (i == 0)
            {
                Destroy(spawnedGameObject.GetComponent<CharacterJoint>());
            }
            else
            {
                spawnedGameObject.GetComponent<CharacterJoint>().connectedBody = parentObject.transform.Find(
                    (parentObject.transform.childCount - 1).ToString()).GetComponent<Rigidbody>();
            }
        }
    }
}
