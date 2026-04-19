using System;
using System.Collections.Generic;

public class CommandEnemyInvoker
{
    private Queue<ICommand> queue = new Queue<ICommand>();

    public void Enqueue(ICommand command)
    {
        queue.Enqueue(command);
    }

    public void ProcessNext()
    {
        if (queue.Count == 0) return;

        queue.Dequeue().Execute();
    }

    public void ProcessAll()
    {
        while(queue.Count > 0)
        {
            queue.Dequeue().Execute();
        }
    }

    // public void GetCellSkillTarget(int actorCell)
    // {
    //     foreach (var command in queue)
    //     {
    //         command.GetData(actorCell)
    //     }
    // }
}