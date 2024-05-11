using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    #region Events
    public static event Action<Vector2, float> onStartTouch;
    public static event Action<Vector2, float> onEndTouch;
    #endregion

    PlayerInput input;

    protected override void Awake()
    {
        base.Awake();
        input = new PlayerInput();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    public Vector3 ScreenToWorld(Vector3 pos)
    {
        pos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(pos);
    }

    public Vector2 PrimaryPosition()
    {
        return ScreenToWorld(input.Touch.PrimaryPosition.ReadValue<Vector2>());
    }

    // Start is called before the first frame update
    void Start()
    {
        input.Touch.PrimaryContact.started += (ctx) =>
            onStartTouch?.Invoke(PrimaryPosition(), (float)ctx.time);
        input.Touch.PrimaryContact.canceled += (ctx) =>
            onEndTouch?.Invoke(PrimaryPosition(), (float)ctx.time);
    }
}
