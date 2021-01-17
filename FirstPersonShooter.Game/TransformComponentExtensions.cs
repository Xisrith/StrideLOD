using Stride.Core.Mathematics;
using Stride.Engine;

namespace FirstPersonShooter
{
    public static class TransformComponentExtensions
    {
        public static Vector3 GetWorldPosition(this TransformComponent transform)
        {
            return transform.WorldMatrix.TranslationVector;
        }

        public static void SetWorldPosition(this TransformComponent transform, Vector3 position)
        {
            transform.Position = transform.Parent == null ? position : transform.Parent.WorldToLocal(position);
        }
    }
}
