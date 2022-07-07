using System.Collections;
using System.Collections.Generic;
using Messages.Server;
using UnityEngine;
using UnityEngine.Pool;

[System.Serializable]
public class PlayerControllerEffects
{
    public PoolList<Transform> steps = new PoolList<Transform>();
    public PoolList<Transform> onGroundHitEffect = new PoolList<Transform>();

    private ThirdPersonController _controller;
    public void Init(ThirdPersonController controller)
    {
        _controller = controller;
   //     _controller.state.HitGround += OnHitGround;
        steps.GeneratePool();
        onGroundHitEffect.GeneratePool();
    }

    public void OnDestroy()
    {
   //     _controller.state.HitGround -= OnHitGround;

    }
    private void OnHitGround()
    {
        ActiveOnGroundEffect();
    }

    public void SetOnGroundHitMSG(Vector3 pos)
    {
        var effect = onGroundHitEffect.FirstFree;
        if (effect)
        {
            effect.position = pos;
            effect.gameObject.SetActive(true);
        }

    }
    public void ActiveOnGroundEffect()
    {
        SpawnPoofMessage.Send(_controller.transform.position);
    } 
    public void ActiveStepEffectMSG(Vector3 Pos)
    {
        var effect = steps.FirstFree;
        effect.position = Pos;
        effect.gameObject.SetActive(true);
    }
}
