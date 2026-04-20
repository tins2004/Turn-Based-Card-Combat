using System.Collections.Generic;

public class EnemiesRepository : Singleton<EnemiesRepository>, IRepository<int, EnemyPresenter>
{
    private readonly Dictionary<int, EnemyPresenter> _data = new Dictionary<int, EnemyPresenter>();
    
    public void Add(int cell, EnemyPresenter enemyPresenter)
    {
        _data[cell] = enemyPresenter;
    }

    public EnemyPresenter Get(int cell)
    {
        return _data.TryGetValue(cell, out var enemyPresenter) ? enemyPresenter : null;
    }

    public Dictionary<int, EnemyPresenter> GetAllDataEnemies()
    {
        return _data;    
    }
    
    public int GetCellByEnemyPresenter(EnemyPresenter enemyPresenter)
    {
        foreach (var pair in _data)
        {
            if (pair.Value == enemyPresenter)
            {
                return pair.Key;
            }
        }

        return -1;
    }

    public void Remove(int cell)
    {
        if (_data.ContainsKey(cell))
        {
            _data.Remove(cell);
        }
    }

    public bool Exists(int cell)
    {
       return _data.ContainsKey(cell);
    }

    public void Clear()
    {
        _data.Clear();
    }
}
