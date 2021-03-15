using MoneyHealth;
using Towers;
using UnityEngine;


public class TowerSpawner : MonoBehaviour
{
    [SerializeField] GameObject rangeCircle;

    public bool IsOccupied {  get; private set; }
    public Tower CurrentTower { get { return tower; } }
    protected Tower tower;

    public void FreeSpawnPosition()
    {
        IsOccupied = false;
    }

    #region Spawn Methods
    public virtual void Spawn(Tower tr, TargetingOptions option)
    {
        if(MoneyContoller.Instance.Money >= tr.Stats.InitialPrice)
        {
            MoneyContoller.Instance.RemoveMoney(tr.Stats.InitialPrice);
            IsOccupied = true;
            GameObject obj = ObjectPooler.Instance.GetPooledObject(tr.gameObject.tag);
            tower = obj.GetComponent<Tower>();
            tower.TargetPriority = option;
            obj.transform.position = transform.position;
            obj.SetActive(true);
        }

    }

    public virtual void Spawn(TowerStats tr, TargetingOptions option)
    {
        if (MoneyContoller.Instance.Money >= tr.InitialPrice)
        {
            MoneyContoller.Instance.RemoveMoney(tr.InitialPrice);
            IsOccupied = true;
            GameObject obj = ObjectPooler.Instance.GetPooledObject(tr.TowerTag);
            tower = obj.GetComponent<Tower>();
            tower.TargetPriority = option;
            obj.transform.position = transform.position;
            obj.SetActive(true);
        }

    }
    #endregion
}
