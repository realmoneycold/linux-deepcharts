using System.Numerics;
using System;

namespace SharpDX
{
    public struct Vector2 : IEquatable<Vector2>
    {
        public float X, Y;

        public static Vector2 Zero => new Vector2(0f);
        public static Vector2 One => new Vector2(1f);
        public static Vector2 UnitX => new Vector2(1f, 0f);
        public static Vector2 UnitY => new Vector2(0f, 1f);

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector2(float value)
        {
            X = value;
            Y = value;
        }

        public float Length()
        {
            return (float)Math.Sqrt(X * X + Y * Y);
        }

        public float LengthSquared()
        {
            return X * X + Y * Y;
        }

        public void Normalize()
        {
            float length = Length();
            if (length > 0f)
            {
                X /= length;
                Y /= length;
            }
        }

        public static Vector2 Normalize(Vector2 value)
        {
            value.Normalize();
            return value;
        }

        public static float Dot(Vector2 a, Vector2 b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        public static float Distance(Vector2 a, Vector2 b)
        {
            return (float)Math.Sqrt(DistanceSquared(a, b));
        }

        public static float DistanceSquared(Vector2 a, Vector2 b)
        {
            float dx = a.X - b.X;
            float dy = a.Y - b.Y;
            return dx * dx + dy * dy;
        }

        public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
        {
            return new Vector2(a.X + (b.X - a.X) * t, a.Y + (b.Y - a.Y) * t);
        }

        public static Vector2 Transform(Vector2 position, Matrix matrix)
        {
            return new Vector2(
                position.X * matrix.M11 + position.Y * matrix.M21 + matrix.M41,
                position.X * matrix.M12 + position.Y * matrix.M22 + matrix.M42
            );
        }

        public static implicit operator System.Numerics.Vector2(Vector2 value)
        {
            return new System.Numerics.Vector2(value.X, value.Y);
        }

        public static implicit operator Vector2(System.Numerics.Vector2 value)
        {
            return new Vector2(value.X, value.Y);
        }

        public static explicit operator System.Numerics.Vector3(Vector2 value)
        {
            return new System.Numerics.Vector3(value.X, value.Y, 0f);
        }

        public static explicit operator Vector2(System.Numerics.Vector3 value)
        {
            return new Vector2(value.X, value.Y);
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X - b.X, a.Y - b.Y);
        }

        public static Vector2 operator -(Vector2 value)
        {
            return new Vector2(-value.X, -value.Y);
        }

        public static Vector2 operator *(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X * b.X, a.Y * b.Y);
        }

        public static Vector2 operator *(Vector2 value, float scale)
        {
            return new Vector2(value.X * scale, value.Y * scale);
        }

        public static Vector2 operator *(float scale, Vector2 value)
        {
            return new Vector2(value.X * scale, value.Y * scale);
        }

        public static Vector2 operator /(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X / b.X, a.Y / b.Y);
        }

        public static Vector2 operator /(Vector2 value, float scale)
        {
            return new Vector2(value.X / scale, value.Y / scale);
        }

        public static bool operator ==(Vector2 a, Vector2 b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Vector2 a, Vector2 b)
        {
            return !(a == b);
        }

        public bool Equals(Vector2 other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override bool Equals(object obj)
        {
            return obj is Vector2 other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }

    public struct Vector3 : IEquatable<Vector3>
    {
        public float X, Y, Z;

        public static Vector3 Zero => new Vector3(0f);
        public static Vector3 One => new Vector3(1f);
        public static Vector3 UnitX => new Vector3(1f, 0f, 0f);
        public static Vector3 UnitY => new Vector3(0f, 1f, 0f);
        public static Vector3 UnitZ => new Vector3(0f, 0f, 1f);
        public static Vector3 Up => new Vector3(0f, 1f, 0f);
        public static Vector3 Down => new Vector3(0f, -1f, 0f);
        public static Vector3 Left => new Vector3(-1f, 0f, 0f);
        public static Vector3 Right => new Vector3(1f, 0f, 0f);
        public static Vector3 Forward => new Vector3(0f, 0f, -1f);
        public static Vector3 Backward => new Vector3(0f, 0f, 1f);

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3(float value)
        {
            X = value;
            Y = value;
            Z = value;
        }

        public Vector3(Vector2 value, float z)
        {
            X = value.X;
            Y = value.Y;
            Z = z;
        }

        public float Length()
        {
            return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public float LengthSquared()
        {
            return X * X + Y * Y + Z * Z;
        }

        public void Normalize()
        {
            float length = Length();
            if (length > 0f)
            {
                X /= length;
                Y /= length;
                Z /= length;
            }
        }

        public static Vector3 Normalize(Vector3 value)
        {
            value.Normalize();
            return value;
        }

        public static float Dot(Vector3 a, Vector3 b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        public static Vector3 Cross(Vector3 a, Vector3 b)
        {
            return new Vector3(
                a.Y * b.Z - a.Z * b.Y,
                a.Z * b.X - a.X * b.Z,
                a.X * b.Y - a.Y * b.X
            );
        }

        public static float Distance(Vector3 a, Vector3 b)
        {
            return (float)Math.Sqrt(DistanceSquared(a, b));
        }

        public static float DistanceSquared(Vector3 a, Vector3 b)
        {
            float dx = a.X - b.X;
            float dy = a.Y - b.Y;
            float dz = a.Z - b.Z;
            return dx * dx + dy * dy + dz * dz;
        }

        public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
        {
            return new Vector3(
                a.X + (b.X - a.X) * t,
                a.Y + (b.Y - a.Y) * t,
                a.Z + (b.Z - a.Z) * t
            );
        }

        public static Vector3 Transform(Vector3 position, Matrix matrix)
        {
            return new Vector3(
                position.X * matrix.M11 + position.Y * matrix.M21 + position.Z * matrix.M31 + matrix.M41,
                position.X * matrix.M12 + position.Y * matrix.M22 + position.Z * matrix.M32 + matrix.M42,
                position.X * matrix.M13 + position.Y * matrix.M23 + position.Z * matrix.M33 + matrix.M43
            );
        }

        public static Vector3 TransformNormal(Vector3 normal, Matrix matrix)
        {
            return new Vector3(
                normal.X * matrix.M11 + normal.Y * matrix.M21 + normal.Z * matrix.M31,
                normal.X * matrix.M12 + normal.Y * matrix.M22 + normal.Z * matrix.M32,
                normal.X * matrix.M13 + normal.Y * matrix.M23 + normal.Z * matrix.M33
            );
        }

        public static implicit operator System.Numerics.Vector3(Vector3 value)
        {
            return new System.Numerics.Vector3(value.X, value.Y, value.Z);
        }

        public static implicit operator Vector3(System.Numerics.Vector3 value)
        {
            return new Vector3(value.X, value.Y, value.Z);
        }

        public static explicit operator System.Numerics.Vector2(Vector3 value)
        {
            return new System.Numerics.Vector2(value.X, value.Y);
        }

        public static explicit operator Vector3(System.Numerics.Vector2 value)
        {
            return new Vector3(value.X, value.Y, 0f);
        }

        public static explicit operator System.Numerics.Vector4(Vector3 value)
        {
            return new System.Numerics.Vector4(value.X, value.Y, value.Z, 1f);
        }

        public static explicit operator Vector3(System.Numerics.Vector4 value)
        {
            return new Vector3(value.X, value.Y, value.Z);
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vector3 operator -(Vector3 value)
        {
            return new Vector3(-value.X, -value.Y, -value.Z);
        }

        public static Vector3 operator *(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        }

        public static Vector3 operator *(Vector3 value, float scale)
        {
            return new Vector3(value.X * scale, value.Y * scale, value.Z * scale);
        }

        public static Vector3 operator *(float scale, Vector3 value)
        {
            return new Vector3(value.X * scale, value.Y * scale, value.Z * scale);
        }

        public static Vector3 operator /(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
        }

        public static Vector3 operator /(Vector3 value, float scale)
        {
            return new Vector3(value.X / scale, value.Y / scale, value.Z / scale);
        }

        public static bool operator ==(Vector3 a, Vector3 b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        public static bool operator !=(Vector3 a, Vector3 b)
        {
            return !(a == b);
        }

        public bool Equals(Vector3 other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
        }

        public override bool Equals(object obj)
        {
            return obj is Vector3 other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }
    }

    public struct Vector4 : IEquatable<Vector4>
    {
        public float X, Y, Z, W;

        public static Vector4 Zero => new Vector4(0f);
        public static Vector4 One => new Vector4(1f);
        public static Vector4 UnitX => new Vector4(1f, 0f, 0f, 0f);
        public static Vector4 UnitY => new Vector4(0f, 1f, 0f, 0f);
        public static Vector4 UnitZ => new Vector4(0f, 0f, 1f, 0f);
        public static Vector4 UnitW => new Vector4(0f, 0f, 0f, 1f);

        public Vector4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Vector4(float value)
        {
            X = value;
            Y = value;
            Z = value;
            W = value;
        }

        public Vector4(Vector2 value, float z, float w)
        {
            X = value.X;
            Y = value.Y;
            Z = z;
            W = w;
        }

        public Vector4(Vector3 value, float w)
        {
            X = value.X;
            Y = value.Y;
            Z = value.Z;
            W = w;
        }

        public float Length()
        {
            return (float)Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
        }

        public float LengthSquared()
        {
            return X * X + Y * Y + Z * Z + W * W;
        }

        public void Normalize()
        {
            float length = Length();
            if (length > 0f)
            {
                X /= length;
                Y /= length;
                Z /= length;
                W /= length;
            }
        }

        public static Vector4 Normalize(Vector4 value)
        {
            value.Normalize();
            return value;
        }

        public static float Dot(Vector4 a, Vector4 b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z + a.W * b.W;
        }

        public static float Distance(Vector4 a, Vector4 b)
        {
            return (float)Math.Sqrt(DistanceSquared(a, b));
        }

        public static float DistanceSquared(Vector4 a, Vector4 b)
        {
            float dx = a.X - b.X;
            float dy = a.Y - b.Y;
            float dz = a.Z - b.Z;
            float dw = a.W - b.W;
            return dx * dx + dy * dy + dz * dz + dw * dw;
        }

        public static Vector4 Lerp(Vector4 a, Vector4 b, float t)
        {
            return new Vector4(
                a.X + (b.X - a.X) * t,
                a.Y + (b.Y - a.Y) * t,
                a.Z + (b.Z - a.Z) * t,
                a.W + (b.W - a.W) * t
            );
        }

        public static Vector4 Transform(Vector4 vector, Matrix matrix)
        {
            return new Vector4(
                vector.X * matrix.M11 + vector.Y * matrix.M21 + vector.Z * matrix.M31 + vector.W * matrix.M41,
                vector.X * matrix.M12 + vector.Y * matrix.M22 + vector.Z * matrix.M32 + vector.W * matrix.M42,
                vector.X * matrix.M13 + vector.Y * matrix.M23 + vector.Z * matrix.M33 + vector.W * matrix.M43,
                vector.X * matrix.M14 + vector.Y * matrix.M24 + vector.Z * matrix.M34 + vector.W * matrix.M44
            );
        }

        public static implicit operator System.Numerics.Vector4(Vector4 value)
        {
            return new System.Numerics.Vector4(value.X, value.Y, value.Z, value.W);
        }

        public static implicit operator Vector4(System.Numerics.Vector4 value)
        {
            return new Vector4(value.X, value.Y, value.Z, value.W);
        }

        public static explicit operator System.Numerics.Vector3(Vector4 value)
        {
            return new System.Numerics.Vector3(value.X, value.Y, value.Z);
        }

        public static explicit operator Vector4(System.Numerics.Vector3 value)
        {
            return new Vector4(value.X, value.Y, value.Z, 1f);
        }

        public static Vector4 operator +(Vector4 a, Vector4 b)
        {
            return new Vector4(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
        }

        public static Vector4 operator -(Vector4 a, Vector4 b)
        {
            return new Vector4(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
        }

        public static Vector4 operator -(Vector4 value)
        {
            return new Vector4(-value.X, -value.Y, -value.Z, -value.W);
        }

        public static Vector4 operator *(Vector4 a, Vector4 b)
        {
            return new Vector4(a.X * b.X, a.Y * b.Y, a.Z * b.Z, a.W * b.W);
        }

        public static Vector4 operator *(Vector4 value, float scale)
        {
            return new Vector4(value.X * scale, value.Y * scale, value.Z * scale, value.W * scale);
        }

        public static Vector4 operator *(float scale, Vector4 value)
        {
            return new Vector4(value.X * scale, value.Y * scale, value.Z * scale, value.W * scale);
        }

        public static Vector4 operator /(Vector4 a, Vector4 b)
        {
            return new Vector4(a.X / b.X, a.Y / b.Y, a.Z / b.Z, a.W / b.W);
        }

        public static Vector4 operator /(Vector4 value, float scale)
        {
            return new Vector4(value.X / scale, value.Y / scale, value.Z / scale, value.W / scale);
        }

        public static bool operator ==(Vector4 a, Vector4 b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z && a.W == b.W;
        }

        public static bool operator !=(Vector4 a, Vector4 b)
        {
            return !(a == b);
        }

        public bool Equals(Vector4 other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z) && W.Equals(other.W);
        }

        public override bool Equals(object obj)
        {
            return obj is Vector4 other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z, W);
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z}, {W})";
        }
    }

    public struct Matrix : IEquatable<Matrix>
    {
        public float M11, M12, M13, M14;
        public float M21, M22, M23, M24;
        public float M31, M32, M33, M34;
        public float M41, M42, M43, M44;

        public static Matrix Identity
        {
            get
            {
                Matrix m = new Matrix();
                m.M11 = 1f;
                m.M22 = 1f;
                m.M33 = 1f;
                m.M44 = 1f;
                return m;
            }
        }

        public static Matrix Multiply(Matrix a, Matrix b)
        {
            Matrix result = new Matrix();
            result.M11 = a.M11 * b.M11 + a.M12 * b.M21 + a.M13 * b.M31 + a.M14 * b.M41;
            result.M12 = a.M11 * b.M12 + a.M12 * b.M22 + a.M13 * b.M32 + a.M14 * b.M42;
            result.M13 = a.M11 * b.M13 + a.M12 * b.M23 + a.M13 * b.M33 + a.M14 * b.M43;
            result.M14 = a.M11 * b.M14 + a.M12 * b.M24 + a.M13 * b.M34 + a.M14 * b.M44;

            result.M21 = a.M21 * b.M11 + a.M22 * b.M21 + a.M23 * b.M31 + a.M24 * b.M41;
            result.M22 = a.M21 * b.M12 + a.M22 * b.M22 + a.M23 * b.M32 + a.M24 * b.M42;
            result.M23 = a.M21 * b.M13 + a.M22 * b.M23 + a.M23 * b.M33 + a.M24 * b.M43;
            result.M24 = a.M21 * b.M14 + a.M22 * b.M24 + a.M23 * b.M34 + a.M24 * b.M44;

            result.M31 = a.M31 * b.M11 + a.M32 * b.M21 + a.M33 * b.M31 + a.M34 * b.M41;
            result.M32 = a.M31 * b.M12 + a.M32 * b.M22 + a.M33 * b.M32 + a.M34 * b.M42;
            result.M33 = a.M31 * b.M13 + a.M32 * b.M23 + a.M33 * b.M33 + a.M34 * b.M43;
            result.M34 = a.M31 * b.M14 + a.M32 * b.M24 + a.M33 * b.M34 + a.M34 * b.M44;

            result.M41 = a.M41 * b.M11 + a.M42 * b.M21 + a.M43 * b.M31 + a.M44 * b.M41;
            result.M42 = a.M41 * b.M12 + a.M42 * b.M22 + a.M43 * b.M32 + a.M44 * b.M42;
            result.M43 = a.M41 * b.M13 + a.M42 * b.M23 + a.M43 * b.M33 + a.M44 * b.M43;
            result.M44 = a.M41 * b.M14 + a.M42 * b.M24 + a.M43 * b.M34 + a.M44 * b.M44;
            return result;
        }

        public static Matrix Transpose(Matrix m)
        {
            Matrix result = new Matrix();
            result.M11 = m.M11;
            result.M12 = m.M21;
            result.M13 = m.M31;
            result.M14 = m.M41;
            result.M21 = m.M12;
            result.M22 = m.M22;
            result.M23 = m.M32;
            result.M24 = m.M42;
            result.M31 = m.M13;
            result.M32 = m.M23;
            result.M33 = m.M33;
            result.M34 = m.M43;
            result.M41 = m.M14;
            result.M42 = m.M24;
            result.M43 = m.M34;
            result.M44 = m.M44;
            return result;
        }

        public static Matrix Invert(Matrix m)
        {
            float det =
                m.M11 * (m.M22 * (m.M33 * m.M44 - m.M34 * m.M43) -
                         m.M23 * (m.M32 * m.M44 - m.M34 * m.M42) +
                         m.M24 * (m.M32 * m.M43 - m.M33 * m.M42)) -
                m.M12 * (m.M21 * (m.M33 * m.M44 - m.M34 * m.M43) -
                         m.M23 * (m.M31 * m.M44 - m.M34 * m.M41) +
                         m.M24 * (m.M31 * m.M43 - m.M33 * m.M41)) +
                m.M13 * (m.M21 * (m.M32 * m.M44 - m.M34 * m.M42) -
                         m.M22 * (m.M31 * m.M44 - m.M34 * m.M41) +
                         m.M24 * (m.M31 * m.M42 - m.M32 * m.M41)) -
                m.M14 * (m.M21 * (m.M32 * m.M43 - m.M33 * m.M42) -
                         m.M22 * (m.M31 * m.M43 - m.M33 * m.M41) +
                         m.M23 * (m.M31 * m.M42 - m.M32 * m.M41));

            if (Math.Abs(det) < 1e-10f)
                return Identity;

            float invDet = 1f / det;

            Matrix result = new Matrix();
            result.M11 = (m.M22 * (m.M33 * m.M44 - m.M34 * m.M43) - m.M23 * (m.M32 * m.M44 - m.M34 * m.M42) + m.M24 * (m.M32 * m.M43 - m.M33 * m.M42)) * invDet;
            result.M12 = -(m.M12 * (m.M33 * m.M44 - m.M34 * m.M43) - m.M13 * (m.M32 * m.M44 - m.M34 * m.M42) + m.M14 * (m.M32 * m.M43 - m.M33 * m.M42)) * invDet;
            result.M13 = (m.M12 * (m.M23 * m.M44 - m.M24 * m.M43) - m.M13 * (m.M22 * m.M44 - m.M24 * m.M42) + m.M14 * (m.M22 * m.M43 - m.M23 * m.M42)) * invDet;
            result.M14 = -(m.M12 * (m.M23 * m.M34 - m.M24 * m.M33) - m.M13 * (m.M22 * m.M34 - m.M24 * m.M32) + m.M14 * (m.M22 * m.M33 - m.M23 * m.M32)) * invDet;

            result.M21 = -(m.M21 * (m.M33 * m.M44 - m.M34 * m.M43) - m.M23 * (m.M31 * m.M44 - m.M34 * m.M41) + m.M24 * (m.M31 * m.M43 - m.M33 * m.M41)) * invDet;
            result.M22 = (m.M11 * (m.M33 * m.M44 - m.M34 * m.M43) - m.M13 * (m.M31 * m.M44 - m.M34 * m.M41) + m.M14 * (m.M31 * m.M43 - m.M33 * m.M41)) * invDet;
            result.M23 = -(m.M11 * (m.M23 * m.M44 - m.M24 * m.M43) - m.M13 * (m.M21 * m.M44 - m.M24 * m.M41) + m.M14 * (m.M21 * m.M43 - m.M23 * m.M41)) * invDet;
            result.M24 = (m.M11 * (m.M23 * m.M34 - m.M24 * m.M33) - m.M13 * (m.M21 * m.M34 - m.M24 * m.M31) + m.M14 * (m.M21 * m.M33 - m.M23 * m.M31)) * invDet;

            result.M31 = (m.M21 * (m.M32 * m.M44 - m.M34 * m.M42) - m.M22 * (m.M31 * m.M44 - m.M34 * m.M41) + m.M24 * (m.M31 * m.M42 - m.M32 * m.M41)) * invDet;
            result.M32 = -(m.M11 * (m.M32 * m.M44 - m.M34 * m.M42) - m.M12 * (m.M31 * m.M44 - m.M34 * m.M41) + m.M14 * (m.M31 * m.M42 - m.M32 * m.M41)) * invDet;
            result.M33 = (m.M11 * (m.M22 * m.M44 - m.M24 * m.M42) - m.M12 * (m.M21 * m.M44 - m.M24 * m.M41) + m.M14 * (m.M21 * m.M42 - m.M22 * m.M41)) * invDet;
            result.M34 = -(m.M11 * (m.M22 * m.M34 - m.M24 * m.M32) - m.M12 * (m.M21 * m.M34 - m.M24 * m.M31) + m.M14 * (m.M21 * m.M32 - m.M22 * m.M31)) * invDet;

            result.M41 = -(m.M21 * (m.M32 * m.M43 - m.M33 * m.M42) - m.M22 * (m.M31 * m.M43 - m.M33 * m.M41) + m.M23 * (m.M31 * m.M42 - m.M32 * m.M41)) * invDet;
            result.M42 = (m.M11 * (m.M32 * m.M43 - m.M33 * m.M42) - m.M12 * (m.M31 * m.M43 - m.M33 * m.M41) + m.M13 * (m.M31 * m.M42 - m.M32 * m.M41)) * invDet;
            result.M43 = -(m.M11 * (m.M22 * m.M43 - m.M23 * m.M42) - m.M12 * (m.M21 * m.M43 - m.M23 * m.M41) + m.M13 * (m.M21 * m.M42 - m.M22 * m.M41)) * invDet;
            result.M44 = (m.M11 * (m.M22 * m.M33 - m.M23 * m.M32) - m.M12 * (m.M21 * m.M33 - m.M23 * m.M31) + m.M13 * (m.M21 * m.M32 - m.M22 * m.M31)) * invDet;

            return result;
        }

        public static Matrix Translation(float x, float y, float z)
        {
            Matrix m = Identity;
            m.M41 = x;
            m.M42 = y;
            m.M43 = z;
            return m;
        }

        public static Matrix Translation(Vector3 value)
        {
            return Translation(value.X, value.Y, value.Z);
        }

        public static Matrix RotationX(float radians)
        {
            Matrix m = Identity;
            float cos = (float)Math.Cos(radians);
            float sin = (float)Math.Sin(radians);
            m.M22 = cos;
            m.M23 = sin;
            m.M32 = -sin;
            m.M33 = cos;
            return m;
        }

        public static Matrix RotationY(float radians)
        {
            Matrix m = Identity;
            float cos = (float)Math.Cos(radians);
            float sin = (float)Math.Sin(radians);
            m.M11 = cos;
            m.M13 = -sin;
            m.M31 = sin;
            m.M33 = cos;
            return m;
        }

        public static Matrix RotationZ(float radians)
        {
            Matrix m = Identity;
            float cos = (float)Math.Cos(radians);
            float sin = (float)Math.Sin(radians);
            m.M11 = cos;
            m.M12 = sin;
            m.M21 = -sin;
            m.M22 = cos;
            return m;
        }

        public static Matrix Scaling(float x, float y, float z)
        {
            Matrix m = Identity;
            m.M11 = x;
            m.M22 = y;
            m.M33 = z;
            return m;
        }

        public static Matrix Scaling(Vector3 value)
        {
            return Scaling(value.X, value.Y, value.Z);
        }

        public static Matrix Scaling(float scale)
        {
            return Scaling(scale, scale, scale);
        }

        public static implicit operator System.Numerics.Matrix4x4(Matrix value)
        {
            return new Matrix4x4(
                value.M11, value.M12, value.M13, value.M14,
                value.M21, value.M22, value.M23, value.M24,
                value.M31, value.M32, value.M33, value.M34,
                value.M41, value.M42, value.M43, value.M44
            );
        }

        public static implicit operator Matrix(System.Numerics.Matrix4x4 value)
        {
            Matrix m = new Matrix();
            m.M11 = value.M11;
            m.M12 = value.M12;
            m.M13 = value.M13;
            m.M14 = value.M14;
            m.M21 = value.M21;
            m.M22 = value.M22;
            m.M23 = value.M23;
            m.M24 = value.M24;
            m.M31 = value.M31;
            m.M32 = value.M32;
            m.M33 = value.M33;
            m.M34 = value.M34;
            m.M41 = value.M41;
            m.M42 = value.M42;
            m.M43 = value.M43;
            m.M44 = value.M44;
            return m;
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            return Multiply(a, b);
        }

        public static bool operator ==(Matrix a, Matrix b)
        {
            return
                a.M11 == b.M11 && a.M12 == b.M12 && a.M13 == b.M13 && a.M14 == b.M14 &&
                a.M21 == b.M21 && a.M22 == b.M22 && a.M23 == b.M23 && a.M24 == b.M24 &&
                a.M31 == b.M31 && a.M32 == b.M32 && a.M33 == b.M33 && a.M34 == b.M34 &&
                a.M41 == b.M41 && a.M42 == b.M42 && a.M43 == b.M43 && a.M44 == b.M44;
        }

        public static bool operator !=(Matrix a, Matrix b)
        {
            return !(a == b);
        }

        public bool Equals(Matrix other)
        {
            return
                M11.Equals(other.M11) && M12.Equals(other.M12) && M13.Equals(other.M13) && M14.Equals(other.M14) &&
                M21.Equals(other.M21) && M22.Equals(other.M22) && M23.Equals(other.M23) && M24.Equals(other.M24) &&
                M31.Equals(other.M31) && M32.Equals(other.M32) && M33.Equals(other.M33) && M34.Equals(other.M34) &&
                M41.Equals(other.M41) && M42.Equals(other.M42) && M43.Equals(other.M43) && M44.Equals(other.M44);
        }

        public override bool Equals(object obj)
        {
            return obj is Matrix other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(M11, M12, M13, M14, M21, M22, M23, M24, M31, M32, M33, M34, M41, M42, M43, M44);
        }

        public override string ToString()
        {
            return $"[{M11}, {M12}, {M13}, {M14}]\n[{M21}, {M22}, {M23}, {M24}]\n[{M31}, {M32}, {M33}, {M34}]\n[{M41}, {M42}, {M43}, {M44}]";
        }
    }

    public struct Matrix3x2 : IEquatable<Matrix3x2>
    {
        public float M11, M12;
        public float M21, M22;
        public float M31, M32;

        public static Matrix3x2 Identity
        {
            get
            {
                Matrix3x2 m = new Matrix3x2();
                m.M11 = 1f;
                m.M22 = 1f;
                return m;
            }
        }

        public static Matrix3x2 Translation(float x, float y)
        {
            Matrix3x2 m = Identity;
            m.M31 = x;
            m.M32 = y;
            return m;
        }

        public static Matrix3x2 Translation(Vector2 value)
        {
            return Translation(value.X, value.Y);
        }

        public static Matrix3x2 Rotation(float radians)
        {
            Matrix3x2 m = Identity;
            float cos = (float)Math.Cos(radians);
            float sin = (float)Math.Sin(radians);
            m.M11 = cos;
            m.M12 = sin;
            m.M21 = -sin;
            m.M22 = cos;
            return m;
        }

        public static Matrix3x2 Scaling(float x, float y)
        {
            Matrix3x2 m = Identity;
            m.M11 = x;
            m.M22 = y;
            return m;
        }

        public static Matrix3x2 Scaling(Vector2 value)
        {
            return Scaling(value.X, value.Y);
        }

        public static Matrix3x2 Scaling(float scale)
        {
            return Scaling(scale, scale);
        }

        public static Matrix3x2 Multiply(Matrix3x2 a, Matrix3x2 b)
        {
            Matrix3x2 result = new Matrix3x2();
            result.M11 = a.M11 * b.M11 + a.M12 * b.M21;
            result.M12 = a.M11 * b.M12 + a.M12 * b.M22;
            result.M21 = a.M21 * b.M11 + a.M22 * b.M21;
            result.M22 = a.M21 * b.M12 + a.M22 * b.M22;
            result.M31 = a.M31 * b.M11 + a.M32 * b.M21 + b.M31;
            result.M32 = a.M31 * b.M12 + a.M32 * b.M22 + b.M32;
            return result;
        }

        public static implicit operator System.Numerics.Matrix3x2(Matrix3x2 value)
        {
            return new System.Numerics.Matrix3x2(
                value.M11, value.M12,
                value.M21, value.M22,
                value.M31, value.M32
            );
        }

        public static implicit operator Matrix3x2(System.Numerics.Matrix3x2 value)
        {
            Matrix3x2 m = new Matrix3x2();
            m.M11 = value.M11;
            m.M12 = value.M12;
            m.M21 = value.M21;
            m.M22 = value.M22;
            m.M31 = value.M31;
            m.M32 = value.M32;
            return m;
        }

        public static Matrix3x2 operator *(Matrix3x2 a, Matrix3x2 b)
        {
            return Multiply(a, b);
        }

        public static bool operator ==(Matrix3x2 a, Matrix3x2 b)
        {
            return
                a.M11 == b.M11 && a.M12 == b.M12 &&
                a.M21 == b.M21 && a.M22 == b.M22 &&
                a.M31 == b.M31 && a.M32 == b.M32;
        }

        public static bool operator !=(Matrix3x2 a, Matrix3x2 b)
        {
            return !(a == b);
        }

        public bool Equals(Matrix3x2 other)
        {
            return
                M11.Equals(other.M11) && M12.Equals(other.M12) &&
                M21.Equals(other.M21) && M22.Equals(other.M22) &&
                M31.Equals(other.M31) && M32.Equals(other.M32);
        }

        public override bool Equals(object obj)
        {
            return obj is Matrix3x2 other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(M11, M12, M21, M22, M31, M32);
        }

        public override string ToString()
        {
            return $"[{M11}, {M12}]\n[{M21}, {M22}]\n[{M31}, {M32}]";
        }
    }

    public struct Color : IEquatable<Color>
    {
        public byte A, R, G, B;

        public static Color Black => new Color(0xff, 0x00, 0x00, 0x00);
        public static Color White => new Color(0xff, 0xff, 0xff, 0xff);
        public static Color Red => new Color(0xff, 0xff, 0x00, 0x00);
        public static Color Green => new Color(0xff, 0x00, 0xff, 0x00);
        public static Color Blue => new Color(0xff, 0x00, 0x00, 0xff);
        public static Color Yellow => new Color(0xff, 0xff, 0xff, 0x00);
        public static Color Cyan => new Color(0xff, 0x00, 0xff, 0xff);
        public static Color Magenta => new Color(0xff, 0xff, 0x00, 0xff);
        public static Color Gray => new Color(0xff, 0x80, 0x80, 0x80);
        public static Color Transparent => new Color(0x00, 0x00, 0x00, 0x00);

        public Color(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        public Color(uint argb)
        {
            A = (byte)((argb >> 24) & 0xff);
            R = (byte)((argb >> 16) & 0xff);
            G = (byte)((argb >> 8) & 0xff);
            B = (byte)(argb & 0xff);
        }

        public Color(float r, float g, float b, float a = 1f)
        {
            R = (byte)(Math.Clamp(r, 0f, 1f) * 255f);
            G = (byte)(Math.Clamp(g, 0f, 1f) * 255f);
            B = (byte)(Math.Clamp(b, 0f, 1f) * 255f);
            A = (byte)(Math.Clamp(a, 0f, 1f) * 255f);
        }

        public uint PackedValue
        {
            get => (uint)((A << 24) | (R << 16) | (G << 8) | B);
            set
            {
                A = (byte)((value >> 24) & 0xff);
                R = (byte)((value >> 16) & 0xff);
                G = (byte)((value >> 8) & 0xff);
                B = (byte)(value & 0xff);
            }
        }

        public uint ToRgba()
        {
            return (uint)((R << 24) | (G << 16) | (B << 8) | A);
        }

        public static implicit operator Color(uint value)
        {
            return new Color(value);
        }

        public static implicit operator uint(Color value)
        {
            return value.PackedValue;
        }

        public static bool operator ==(Color a, Color b)
        {
            return a.A == b.A && a.R == b.R && a.G == b.G && a.B == b.B;
        }

        public static bool operator !=(Color a, Color b)
        {
            return !(a == b);
        }

        public bool Equals(Color other)
        {
            return A == other.A && R == other.R && G == other.G && B == other.B;
        }

        public override bool Equals(object obj)
        {
            return obj is Color other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (int)PackedValue;
        }

        public override string ToString()
        {
            return $"[A:{A}, R:{R}, G:{G}, B:{B}]";
        }
    }

    public struct Color4 : IEquatable<Color4>
    {
        public float A, R, G, B;

        public Color4(float a, float r, float g, float b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        public Color4(float r, float g, float b)
        {
            A = 1f;
            R = r;
            G = g;
            B = b;
        }

        public Color4(Color color)
        {
            A = color.A / 255f;
            R = color.R / 255f;
            G = color.G / 255f;
            B = color.B / 255f;
        }

        public Color4(Vector4 vector)
        {
            A = vector.W;
            R = vector.X;
            G = vector.Y;
            B = vector.Z;
        }

        public Color4(uint rgba)
        {
            R = ((rgba >> 24) & 0xff) / 255f;
            G = ((rgba >> 16) & 0xff) / 255f;
            B = ((rgba >> 8) & 0xff) / 255f;
            A = (rgba & 0xff) / 255f;
        }

        public Color ToColor()
        {
            return new Color(R, G, B, A);
        }

        public static implicit operator Color4(Color value)
        {
            return new Color4(value);
        }

        public static implicit operator Color4(Vector4 value)
        {
            return new Color4(value);
        }

        public static bool operator ==(Color4 a, Color4 b)
        {
            return a.A == b.A && a.R == b.R && a.G == b.G && a.B == b.B;
        }

        public static bool operator !=(Color4 a, Color4 b)
        {
            return !(a == b);
        }

        public bool Equals(Color4 other)
        {
            return A.Equals(other.A) && R.Equals(other.R) && G.Equals(other.G) && B.Equals(other.B);
        }

        public override bool Equals(object obj)
        {
            return obj is Color4 other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(A, R, G, B);
        }

        public override string ToString()
        {
            return $"[A:{A}, R:{R}, G:{G}, B:{B}]";
        }
    }

    public struct RectangleF : IEquatable<RectangleF>
    {
        public float X, Y, Width, Height;

        public static RectangleF Empty => new RectangleF(0f, 0f, 0f, 0f);

        public RectangleF(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public RectangleF(Vector2 position, Size2F size)
        {
            X = position.X;
            Y = position.Y;
            Width = size.Width;
            Height = size.Height;
        }

        public float Left => X;
        public float Top => Y;
        public float Right => X + Width;
        public float Bottom => Y + Height;

        public Vector2 Location
        {
            get => new Vector2(X, Y);
            set { X = value.X; Y = value.Y; }
        }

        public Size2F Size
        {
            get => new Size2F(Width, Height);
            set { Width = value.Width; Height = value.Height; }
        }

        public bool Contains(float x, float y)
        {
            return x >= Left && x <= Right && y >= Top && y <= Bottom;
        }

        public bool Contains(Vector2 point)
        {
            return Contains(point.X, point.Y);
        }

        public bool Contains(RectangleF rect)
        {
            return Left <= rect.Left && Right >= rect.Right && Top <= rect.Top && Bottom >= rect.Bottom;
        }

        public static RectangleF Intersect(RectangleF a, RectangleF b)
        {
            float left = Math.Max(a.Left, b.Left);
            float top = Math.Max(a.Top, b.Top);
            float right = Math.Min(a.Right, b.Right);
            float bottom = Math.Min(a.Bottom, b.Bottom);
            if (right >= left && bottom >= top)
                return new RectangleF(left, top, right - left, bottom - top);
            return Empty;
        }

        public static RectangleF Union(RectangleF a, RectangleF b)
        {
            float left = Math.Min(a.Left, b.Left);
            float top = Math.Min(a.Top, b.Top);
            float right = Math.Max(a.Right, b.Right);
            float bottom = Math.Max(a.Bottom, b.Bottom);
            return new RectangleF(left, top, right - left, bottom - top);
        }

        public static bool operator ==(RectangleF a, RectangleF b)
        {
            return a.X == b.X && a.Y == b.Y && a.Width == b.Width && a.Height == b.Height;
        }

        public static bool operator !=(RectangleF a, RectangleF b)
        {
            return !(a == b);
        }

        public bool Equals(RectangleF other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && Width.Equals(other.Width) && Height.Equals(other.Height);
        }

        public override bool Equals(object obj)
        {
            return obj is RectangleF other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Width, Height);
        }

        public override string ToString()
        {
            return $"[X:{X}, Y:{Y}, Width:{Width}, Height:{Height}]";
        }
    }

    public struct Rectangle : IEquatable<Rectangle>
    {
        public int X, Y, Width, Height;

        public static Rectangle Empty => new Rectangle(0, 0, 0, 0);

        public Rectangle(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Rectangle(Point position, Size2 size)
        {
            X = position.X;
            Y = position.Y;
            Width = size.Width;
            Height = size.Height;
        }

        public int Left => X;
        public int Top => Y;
        public int Right => X + Width;
        public int Bottom => Y + Height;

        public Point Location
        {
            get => new Point(X, Y);
            set { X = value.X; Y = value.Y; }
        }

        public Size2 Size
        {
            get => new Size2(Width, Height);
            set { Width = value.Width; Height = value.Height; }
        }

        public bool Contains(int x, int y)
        {
            return x >= Left && x <= Right && y >= Top && y <= Bottom;
        }

        public bool Contains(Point point)
        {
            return Contains(point.X, point.Y);
        }

        public bool Contains(Rectangle rect)
        {
            return Left <= rect.Left && Right >= rect.Right && Top <= rect.Top && Bottom >= rect.Bottom;
        }

        public static Rectangle Intersect(Rectangle a, Rectangle b)
        {
            int left = Math.Max(a.Left, b.Left);
            int top = Math.Max(a.Top, b.Top);
            int right = Math.Min(a.Right, b.Right);
            int bottom = Math.Min(a.Bottom, b.Bottom);
            if (right >= left && bottom >= top)
                return new Rectangle(left, top, right - left, bottom - top);
            return Empty;
        }

        public static Rectangle Union(Rectangle a, Rectangle b)
        {
            int left = Math.Min(a.Left, b.Left);
            int top = Math.Min(a.Top, b.Top);
            int right = Math.Max(a.Right, b.Right);
            int bottom = Math.Max(a.Bottom, b.Bottom);
            return new Rectangle(left, top, right - left, bottom - top);
        }

        public static implicit operator RectangleF(Rectangle value)
        {
            return new RectangleF(value.X, value.Y, value.Width, value.Height);
        }

        public static explicit operator Rectangle(RectangleF value)
        {
            return new Rectangle((int)value.X, (int)value.Y, (int)value.Width, (int)value.Height);
        }

        public static bool operator ==(Rectangle a, Rectangle b)
        {
            return a.X == b.X && a.Y == b.Y && a.Width == b.Width && a.Height == b.Height;
        }

        public static bool operator !=(Rectangle a, Rectangle b)
        {
            return !(a == b);
        }

        public bool Equals(Rectangle other)
        {
            return X == other.X && Y == other.Y && Width == other.Width && Height == other.Height;
        }

        public override bool Equals(object obj)
        {
            return obj is Rectangle other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Width, Height);
        }

        public override string ToString()
        {
            return $"[X:{X}, Y:{Y}, Width:{Width}, Height:{Height}]";
        }
    }

    public struct Size2F : IEquatable<Size2F>
    {
        public float Width, Height;

        public Size2F(float width, float height)
        {
            Width = width;
            Height = height;
        }

        public static Size2F Zero => new Size2F(0f, 0f);

        public static bool operator ==(Size2F a, Size2F b)
        {
            return a.Width == b.Width && a.Height == b.Height;
        }

        public static bool operator !=(Size2F a, Size2F b)
        {
            return !(a == b);
        }

        public bool Equals(Size2F other)
        {
            return Width.Equals(other.Width) && Height.Equals(other.Height);
        }

        public override bool Equals(object obj)
        {
            return obj is Size2F other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Width, Height);
        }

        public override string ToString()
        {
            return $"[Width:{Width}, Height:{Height}]";
        }
    }

    public struct Size2 : IEquatable<Size2>
    {
        public int Width, Height;

        public Size2(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public static Size2 Zero => new Size2(0, 0);

        public static implicit operator Size2F(Size2 value)
        {
            return new Size2F(value.Width, value.Height);
        }

        public static explicit operator Size2(Size2F value)
        {
            return new Size2((int)value.Width, (int)value.Height);
        }

        public static bool operator ==(Size2 a, Size2 b)
        {
            return a.Width == b.Width && a.Height == b.Height;
        }

        public static bool operator !=(Size2 a, Size2 b)
        {
            return !(a == b);
        }

        public bool Equals(Size2 other)
        {
            return Width == other.Width && Height == other.Height;
        }

        public override bool Equals(object obj)
        {
            return obj is Size2 other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Width, Height);
        }

        public override string ToString()
        {
            return $"[Width:{Width}, Height:{Height}]";
        }
    }

    public struct Point : IEquatable<Point>
    {
        public int X, Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Point Zero => new Point(0, 0);

        public static implicit operator PointF(Point value)
        {
            return new PointF(value.X, value.Y);
        }

        public static explicit operator Point(PointF value)
        {
            return new Point((int)value.X, (int)value.Y);
        }

        public static bool operator ==(Point a, Point b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Point a, Point b)
        {
            return !(a == b);
        }

        public bool Equals(Point other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is Point other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public override string ToString()
        {
            return $"[X:{X}, Y:{Y}]";
        }
    }

    public struct PointF : IEquatable<PointF>
    {
        public float X, Y;

        public PointF(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static PointF Zero => new PointF(0f, 0f);

        public static implicit operator Vector2(PointF value)
        {
            return new Vector2(value.X, value.Y);
        }

        public static implicit operator PointF(Vector2 value)
        {
            return new PointF(value.X, value.Y);
        }

        public static bool operator ==(PointF a, PointF b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(PointF a, PointF b)
        {
            return !(a == b);
        }

        public bool Equals(PointF other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override bool Equals(object obj)
        {
            return obj is PointF other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public override string ToString()
        {
            return $"[X:{X}, Y:{Y}]";
        }
    }
}
