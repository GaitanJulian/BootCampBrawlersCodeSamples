using UnityEngine;

public class ObstacleColliderHandler : MonoBehaviour
{
    /*
    private void OnCollisionEnter(Collision other)
    {
        other.gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.SetActive(false);
    }*/

    [SerializeField] private DodgeGameWins dodgeWin;

    public GameObject targetContainer; // El contenedor donde se moverá el objeto

    private void OnCollisionEnter(Collision other)
    {
        MoveObjectAndParents(other.gameObject);
        dodgeWin.PlayersOnScene();
        dodgeWin.ValidIfWin();
    }

    private void OnTriggerEnter(Collider other)
    {
        MoveObjectAndParents(other.gameObject);
        dodgeWin.PlayersOnScene();
        dodgeWin.ValidIfWin();
    }

    private void MoveObjectAndParents(GameObject obj)
    {
        Transform parent = obj.transform.parent;
        int parentsToMove = 3;

        GameObject copiedObject = Instantiate(obj, targetContainer.transform);

        while (parent != null && parentsToMove > 0)
        {
            // Copiar la posición, rotación y escala del objeto original al nuevo objeto
            copiedObject.transform.position = obj.transform.position;
            copiedObject.transform.rotation = obj.transform.rotation;
            copiedObject.transform.localScale = obj.transform.localScale;
            if (parent != obj.transform)
            {
                Destroy(obj);
            }

            obj = parent.gameObject;
            parent = obj.transform.parent;
            parentsToMove--;
        }
    }
}
