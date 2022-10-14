using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Diagnostics.Contracts;

public class LevelController : MonoBehaviour
{
    public Text boxCounter;
    public Text timer;
    public GameObject gameOverScreen;
    public GameObject gameWonScreen;
    public GameObject cratePrefab;
    public GameObject standPrefab;

    public int crateCount = 3;
    //czas do koñca poziomu w sekundach
    float timeLeft;
    List<GameObject> crateList;

    // Start is called before the first frame update
    void Start()
    {
        crateList = new List<GameObject> ();
        createStands(crateCount);
        createBoxes(crateCount);
        timeLeft = 60;
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        timer.text = "Pozosta³y czas: \n" + Mathf.Round(timeLeft).ToString();
        if(timeLeft <= 0)
        {

            gameOver();
        }
        //Debug.Log(Time.timeScale);
        countBoxes();
    }
    void createStands(int count)
    {
        int x, z;
        while(count > 0)
        {
            x = Random.Range(-20, 20);
            z = Random.Range(-20, 20);
            Vector3 newItemPosition = new Vector3(x, 0, z);
            if(!Physics.CheckSphere(newItemPosition, 3))
            {
                Instantiate(standPrefab, newItemPosition, Quaternion.identity);
                count--;
            }
            
        }
        
    }
    void createBoxes(int count)
    {
        int x, z;
        while (count > 0)
        {
            x = Random.Range(-20, 20);
            z = Random.Range(-20, 20);
            Vector3 newItemPosition = new Vector3(x, 0, z);
            if (!Physics.CheckSphere(newItemPosition, 3))
            {
                GameObject crate = Instantiate(cratePrefab, newItemPosition, Quaternion.identity);
                crateList.Add(crate);
                count--;
            }

        }
    }
    void countBoxes()
    {
        int crateTotal = crateList.Count;
        int crateInPlace = 0;
        foreach (GameObject crate in crateList)
        {
            if(crate.transform.parent != null && crate.transform.parent.CompareTag("Stand"))
                crateInPlace++;
        }
        boxCounter.text = "Ustawione skrzynki: " + crateInPlace.ToString() + "/" + crateTotal.ToString();
        if (crateInPlace == crateCount)
            gameWon();
    }
    void gameOver()
    {
        Time.timeScale = 0; //pauzuje gre
        gameOverScreen.SetActive(true);
    }
    void gameWon()
    {
        Time.timeScale = 0; //pauzuje gre
        gameWonScreen.SetActive(true);
    }
    public void restartLevel()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }
}
