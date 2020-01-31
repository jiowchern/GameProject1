using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class DisconnectStage : Regulus.Utility.IStatus
{

    public Action DoneEvent;
    private Regulus.Remote.INotifier<Regulus.Remote.IOnline> notifier;

    public DisconnectStage(Regulus.Remote.INotifier<Regulus.Remote.IOnline> notifier)
    {
        // TODO: Complete member initialization
        this.notifier = notifier;
    }


    void Regulus.Utility.IStatus.Enter()
    {
        if (notifier.Ghosts.Length == 0)
            DoneEvent();

        notifier.Unsupply += notifier_Unsupply;
        notifier.Supply += notifier_Supply;

    }

    void notifier_Unsupply(Regulus.Remote.IOnline obj)
    {
        DoneEvent();
    }

    void notifier_Supply(Regulus.Remote.IOnline obj)
    {
        obj.Disconnect();
    }

    void Regulus.Utility.IStatus.Leave()
    {
        notifier.Unsupply -= notifier_Unsupply;
        notifier.Supply -= notifier_Supply;
    }

    void Regulus.Utility.IStatus.Update()
    {

    }
}
