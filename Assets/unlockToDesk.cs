using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.AI;




public class unlockToDesk : MonoBehaviour
{
    [SerializeField] private GameObject unlockProgressObj;
    [SerializeField] private GameObject newDesk;
    [SerializeField] private Image progressBar;
    [SerializeField] private Text dollarAmount;
    [SerializeField] private int deskPrice, deskRemainPrice;
    [SerializeField] private float progressValue;
    public NavMeshSurface buildNavMesh;
    void Start()
    {
        dollarAmount.text = deskPrice.ToString("C0");
        deskRemainPrice = deskPrice;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && PlayerPrefs.GetInt("dollar") >0)
        {
            progressValue = Mathf.Abs(1f - Calculatedollar() / deskPrice);
            if (PlayerPrefs.GetInt("dollar") > deskPrice)
            {
                PlayerPrefs.SetInt("dollar", PlayerPrefs.GetInt("dollar") - deskPrice);
                deskRemainPrice = 0;
            }
            else
            {
                deskRemainPrice -= PlayerPrefs.GetInt("dollar");
                PlayerPrefs.SetInt("dollar", 0);
            }

            progressBar.fillAmount = progressValue;

            playerManager.playerManagerInstance.moneyCounter.text = PlayerPrefs.GetInt("dollar").ToString("C0");
            dollarAmount.text = deskRemainPrice.ToString("C0");

            if (deskRemainPrice == 0)
            {
                GameObject Desk = Instantiate(newDesk,new Vector3(transform.position.x,0f,transform.position.z),Quaternion.Euler(0f,0f,0f));

                Desk.transform.DOScale(1f, 1f).SetEase(Ease.OutElastic);

                unlockProgressObj.SetActive(false);

                buildNavMesh.BuildNavMesh();
            
            }
        }
        
    }

    private float Calculatedollar()
    {
        return deskRemainPrice - PlayerPrefs.GetInt("dollar");
    }


}
