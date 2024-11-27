using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Headbob : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private bool _enable = true; // Enables or disables head bobbing.
    [SerializeField, Range(0, 0.1f)] private float _Amplitude = 0.015f; // Amplitude of the bobbing motion.
    [SerializeField, Range(0, 30)] private float _frequency = 10.0f; // Frequency of the bobbing motion.
    [SerializeField] private Transform _camera = null; // The camera transform to apply bobbing.
    [SerializeField] private Transform _cameraHolder = null; // Reference for camera focus.

    private float _toggleSpeed = 2.0f; // Minimum speed to trigger bobbing.
    private Vector3 _startPos; // Stores the initial camera position.
    private CharacterController _controller; // Character controller reference for movement checks.

    private void Awake()
    {
        // Initialize the character controller and save the camera's initial position.
        _controller = GetComponent<CharacterController>();
        _startPos = _camera.localPosition;
    }

    void Update()
    {
        // Main update loop to handle head bobbing and resetting position.
        if (!_enable) return;
        CheckMotion();
        ResetPosition();

        // Optionally remove this line to allow free camera movement:
        // _camera.LookAt(FocusTarget());
    }


    private Vector3 FootStepMotion()
    {
        // Creates the head bobbing motion using sine and cosine waves.
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * _frequency) * _Amplitude;
        pos.x += Mathf.Cos(Time.time * _frequency / 2) * _Amplitude * 2;
        return pos;
    }

    private void CheckMotion()
    {
        // Determines if the character is moving and on the ground to apply motion.
        float speed = new Vector3(_controller.velocity.x, 0, _controller.velocity.z).magnitude;
        if (speed < _toggleSpeed || !_controller.isGrounded) return;

        PlayMotion(FootStepMotion());
    }

    private void PlayMotion(Vector3 motion)
    {
        // Applies the calculated motion to the camera's position.
        _camera.localPosition += motion;
    }

    private Vector3 FocusTarget()
    {
        // Calculates a focus point for the camera to look at during movement.
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + _cameraHolder.localPosition.y, transform.position.z);
        pos += _cameraHolder.forward * 15.0f;
        return pos;
    }

    private void ResetPosition()
    {
        // Smoothly resets the camera position to its initial state.
        if (_camera.localPosition != _startPos)
        {
            _camera.localPosition = Vector3.Lerp(_camera.localPosition, _startPos, 1 * Time.deltaTime);
        }
    }
}
