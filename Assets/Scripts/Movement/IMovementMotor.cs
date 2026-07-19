using UnityEngine;

namespace AF
{
    public interface IMovementMotor
    {
        float MoveAmount { get; }
        float RunSpeed { get; }
        Vector3 PlanarVelocity { get; }
        float StoppingDistance { get; set; }
        float RemainingDistance { get; }
        bool HasPath { get; }
        bool IsMotorEnabled { get; }

        void Move(Vector3 direction, float moveAmount);
        void SetDestination(Vector3 worldPosition);
        void Stop();
        void ApplyDisplacement(Vector3 worldDelta);
        void SetMotorEnabled(bool enabled);
    }
}
