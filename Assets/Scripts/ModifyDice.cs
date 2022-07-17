using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ModifyDice : MonoBehaviour
{
   
    private Transform zoomPoint;
    private Vector3 faceHoldPoint = new Vector3(0, 2, -1f);

    float objectSlideSpeed = 15;
    float distanceToOffscreen = 8;

    float objectSpacing = 2.5f;
    float dicerotateSpeed = 60;
    public DicePoolManager dicePoolManager;

    public UnityEvent onDiceModComplete;

    public Transform searchFaceRewardPt;
    private Face searchRewardFace;

    void Start()
    {
        zoomPoint = Camera.main.transform.Find("DiceZoomPoint");
    }

   

    public Pip chooseRandomPip()
    {
        float randomNumberCap = 0;
        foreach(Pip pip in GameManager.instance.pips)
        {
            randomNumberCap += pip.rarity;
        }
        float randomNum = Random.Range(0, randomNumberCap);
        foreach (Pip pip in GameManager.instance.pips)
        {
            randomNum -= pip.rarity;
            if(randomNum <= 0)
            {
                return pip;
            }
        }
        return (GameManager.instance.pips[0]);
    }
    public int choosePipQuantitiy(Pip pip)
    {
        return Random.Range(Mathf.FloorToInt(pip.abundance.x), Mathf.FloorToInt(pip.abundance.y));
    }
    public Face createRandomFace()
    {
        Face newFace = Instantiate(GameManager.instance.getFacePrefab_OfSize(6)).GetComponent<Face>();

        Pip pip = chooseRandomPip();
        int quantity = choosePipQuantitiy(pip);
        for (int j = 0; j < quantity; j++)
        {
            newFace.addPip(pip.type);
        }
        newFace.configureFace(FaceType.pip);
        return newFace;
    }
    public List<Face> CreateSetOfRandomFaces(int count = 3)
    {
        List<Face> faces = new List<Face>();
        for (int i = 0; i < count; i++)
        {
            Face newFace = Instantiate(GameManager.instance.getFacePrefab_OfSize(6)).GetComponent<Face>();

            Pip pip = chooseRandomPip();
            int quantity = choosePipQuantitiy(pip);
            for (int j = 0; j < quantity; j++)
            {
                newFace.addPip(pip.type);
            }

            newFace.configureFace(FaceType.pip);

            faces.Add(newFace);
        }
        return faces;
    }

    public void GenerateSearchRewardFace()
    {
        Debug.Log("Generating die face");
        Face face = createRandomFace();
        face.transform.position = searchFaceRewardPt.position;
        face.transform.rotation = searchFaceRewardPt.rotation;
        face.SetHighlight(true);
        searchRewardFace = face;
    }
    public void ClearSearchRewardFace()
    {
        if (!searchRewardFace.dice) { Destroy(searchRewardFace.gameObject); }
        searchRewardFace = null;
    }
    public void AddSearchRewardFace()
    {
        if(searchRewardFace == null) { Debug.LogWarning("Tried to give the player a search reward when none exists"); return; }
        AddGivenFace(searchRewardFace);
    }
    public void moveTransformsTo_ZoomPoint(List<Transform> objects)
    {
        int count = objects.Count;
        float span = (count - 1) * objectSpacing;
        float startX = (-span / 2);
        for (int i = 0; i < count; i++)
        {
            float newX = startX + objectSpacing * i;
            objects[i].position = zoomPoint.position + new Vector3(newX, 0, 0);
            objects[i].rotation = zoomPoint.rotation;
        }
    }
    public void AddGivenFace(Face face)
    {
        StartCoroutine(_AddGivenFace());
        IEnumerator _AddGivenFace()
        {
            face.transform.position = zoomPoint.position + faceHoldPoint;
            face.transform.rotation = zoomPoint.rotation;
            face.SetHighlight(true);
            yield return new WaitForSeconds(2f);
            yield return slideTransformAlong(face.transform, Vector2.up * distanceToOffscreen);
            StartCoroutine(slideTransformTo(face.transform, zoomPoint.TransformPoint(faceHoldPoint)));

            Transform chosenSocket = null;
            yield return SelectSocketFromDicePool((socket) => { chosenSocket = socket; });

            Dice chosenDice = null;
            foreach (Dice dice in dicePoolManager.diceInPool)
            {
                if (dice.sockets.Contains(chosenSocket))
                { chosenDice = dice; }
            }
            chosenDice.AttatchFace(face, chosenSocket);

            StartCoroutine(ShowOffTransform(chosenDice.diceBody.transform, 4));
            yield return new WaitForSeconds(3f);
            yield return slideTransformAlong(chosenDice.diceBody.transform, Vector2.up * distanceToOffscreen);
            chosenDice.sendHome();
            onDiceModComplete.Invoke();
        }
    }
    public void AddFaceOfPlayersChoice()
    {
        StartCoroutine(_AddFaceOfPlayersChoice());
        IEnumerator _AddFaceOfPlayersChoice()
        {
            Face chosenFace = null;
            yield return SelectNewFace((face) => { chosenFace = face; });

            Transform chosenSocket = null;
            yield return SelectSocketFromDicePool((socket) => { chosenSocket = socket; });

            Dice chosenDice = null;
            foreach (Dice dice in dicePoolManager.diceInPool)
            {
                if (dice.sockets.Contains(chosenSocket))
                { chosenDice = dice; }
            }
            chosenDice.AttatchFace(chosenFace, chosenSocket);

            StartCoroutine(ShowOffTransform(chosenDice.diceBody.transform, 4));
            yield return new WaitForSeconds(3f);
            yield return slideTransformAlong(chosenDice.diceBody.transform, Vector2.up * distanceToOffscreen);
            chosenDice.sendHome();
            onDiceModComplete.Invoke();
        }
    }
    public void AddBugFace()
    {
        StartCoroutine(AddBugFace());
        IEnumerator AddBugFace()
        {
            Face face = Instantiate(GameManager.instance.getFacePrefab_OfSize(6)).GetComponent<Face>();
            face.configureFace(FaceType.isopod);
            face.transform.position = zoomPoint.position + faceHoldPoint;
            face.transform.rotation = zoomPoint.rotation;
            face.SetHighlight(true);

            StartCoroutine(ShowOffTransform(face.transform, 4));
            yield return new WaitForSeconds(3f);
            yield return slideTransformAlong(face.transform, Vector2.up * distanceToOffscreen);
            StartCoroutine(slideTransformTo(face.transform, zoomPoint.TransformPoint(faceHoldPoint)));


            Transform chosenSocket = null;
            yield return SelectSocketFromDicePool((socket) => { chosenSocket = socket; });

            Dice chosenDice = null;
            foreach (Dice dice in dicePoolManager.diceInPool)
            {
                if (dice.sockets.Contains(chosenSocket))
                { chosenDice = dice; }
            }
            chosenDice.AttatchFace(face, chosenSocket);

            StartCoroutine(ShowOffTransform(chosenDice.diceBody.transform, 4));
            yield return new WaitForSeconds(3f);
            yield return slideTransformAlong(chosenDice.diceBody.transform, Vector2.up * distanceToOffscreen);
            chosenDice.sendHome();
            onDiceModComplete.Invoke();
        }
    }
    public IEnumerator ShowOffTransform(Transform transform, float time)
    {
        float endTime = Time.time + time;
        transform.position = zoomPoint.position;

        while (endTime > Time.time)
        {
            transform.Rotate(new Vector3(1, 1, Mathf.Sin(Time.time)) * dicerotateSpeed * Time.deltaTime);
            yield return null;
        }
    }
    public IEnumerator SelectNewFace(System.Action<Face> callback = null)
    {
        yield return null; //wait a frame in case this is happening right away (TODO remove this)
        Face selectedFace = null;

        List<Face> faces = CreateSetOfRandomFaces();

        //Allign all the transforms
        List<Transform> faceTransformList = new List<Transform>();
        foreach (Face face in faces) { faceTransformList.Add(face.transform);}
        moveTransformsTo_ZoomPoint(faceTransformList);

        foreach (Face face in faces)
        {
            face.SetHighlight(true);
        }

        while (selectedFace == null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = GameManager.instance.camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] raycastHits = Physics.RaycastAll(ray, 100f);
                foreach (RaycastHit raycastHit in raycastHits)
                {
                    if (faceTransformList.Contains(raycastHit.transform))
                    {
                        selectedFace = raycastHit.transform.GetComponent<Face>();
                    }
                }
            }
            yield return null;
        }


        //wipe out unused faces
        foreach (Face face in faces)
        {
            if (face != selectedFace) { 
                face.SetHighlight(false);
                yield return new WaitForSeconds(.05f);
                StartCoroutine(slideTransformAlong(face.transform, Vector2.down * distanceToOffscreen,true));
                yield return new WaitForSeconds(.05f);
            }
        }

        yield return new WaitForSeconds(.05f);
        yield return slideTransformTo(selectedFace.transform, zoomPoint.TransformPoint(faceHoldPoint));
        if (callback != null) { callback.Invoke(selectedFace); }
    }
    public IEnumerator SelectSocketFromDicePool(System.Action<Transform> callback = null)
    {
        yield return null; //wait a frame in case this is happening right away (TODO remove this)
        Transform selectedSocket = null;

        
        //freeze dice
        foreach (Dice dice in dicePoolManager.diceInPool)
        { 
            dice.diceBody.GetComponent<Rigidbody>().isKinematic = true;
        }

        //make list of transforms so we can align them on the zoom point
        List<Transform> diceTransformList = new List<Transform>();
        foreach (Dice dice in dicePoolManager.diceInPool) { diceTransformList.Add(dice.diceBody.transform); }

        //Move all thee transforms where they need to go
        moveTransformsTo_ZoomPoint(diceTransformList);

        List<Transform> allSockets = new List<Transform>();
        foreach (Dice dice in dicePoolManager.diceInPool)
        {
            allSockets.AddRange(dice.sockets);
        }

        while (selectedSocket == null) //do not proceed until a dice face is selected.
        {
            foreach (Dice dice in dicePoolManager.diceInPool)
            {
                dice.diceBody.transform.Rotate(new Vector3(1, 1, Mathf.Sin(Time.time)) * dicerotateSpeed * Time.deltaTime);
            }

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = GameManager.instance.camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] raycastHits = Physics.RaycastAll(ray, 100f);
                List<Transform> hitSockets = new List<Transform>();
                foreach (RaycastHit raycastHit in raycastHits)
                {
                    if (allSockets.Contains(raycastHit.transform))
                    {
                        hitSockets.Add(raycastHit.transform);
                    }
                }
                Transform closestSocket = null;
                float closestSocketdist = 1000;
                foreach(Transform socket in hitSockets)
                {
                    float socketDist = (socket.position - Camera.main.transform.position).magnitude;
                    if (socketDist < closestSocketdist)
                    { //closest socket so far
                        closestSocket = socket;
                        closestSocketdist = socketDist;
                    }
                }

                //Filter out bug faces. They don't count as buildable faces
                Face existingFace = closestSocket.GetComponentInChildren<Face>();
                if (existingFace)
                {
                    if (existingFace.faceType == FaceType.isopod)
                    {
                        selectedSocket = null;
                    }
                    else
                    {
                        selectedSocket = closestSocket;
                    }
                }
                else
                {
                    selectedSocket = closestSocket;
                }
            }
            yield return null;
        }

        Dice selectedDice = null;
        foreach (Dice dice in dicePoolManager.diceInPool)
        {
            if (!dice.sockets.Contains(selectedSocket))
            {
                yield return slideTransformAlong(dice.diceBody.transform, Vector2.down * distanceToOffscreen);
                dice.sendHome();
            }
            else { selectedDice = dice;}
        }

        yield return slideTransformTo(selectedDice.diceBody.transform, zoomPoint.TransformPoint(Vector3.zero));
        if (callback != null) { callback.Invoke(selectedSocket); }
    }

    IEnumerator slideTransformAlong(Transform transform, Vector3 path, bool destroyAfter = false)
    {
        Vector3 StartPos = transform.position;
        while ((transform.position - StartPos).magnitude < path.magnitude)
        {
            transform.position += zoomPoint.TransformDirection(path.normalized) * Time.deltaTime * objectSlideSpeed;
            yield return null;
        }
        if (destroyAfter) { Destroy(transform.gameObject); }
    }

    IEnumerator slideTransformTo(Transform transform, Vector3 destination, bool destroyAfter = false)
    {
        Vector3 StartPos = transform.position;
        Vector3 path = destination - transform.position;
        while ((StartPos - transform.position).magnitude < path.magnitude)
        {
            transform.Translate(path.normalized * Time.deltaTime * objectSlideSpeed, Space.World);
            yield return null;
        }
        transform.position = destination;
        if (destroyAfter) { Destroy(transform.gameObject); }
    } 

}
