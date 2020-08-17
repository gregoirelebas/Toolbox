using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolbox
{
    public enum LerpType
    {
        Vector2,
        Vector3,
        Quaternion,
        Int,
        Float,
        Double
    }

    public class LerpPoints
    {
        public Vector2 startVec2 = Vector2.zero;
        public Vector2 endVec2 = Vector2.zero;

        public Vector3 startVec3 = Vector3.zero;
        public Vector3 endVec3 = Vector3.zero;

        public Quaternion startQuat = Quaternion.identity;
        public Quaternion endQuat = Quaternion.identity;

        public int startInt = 0;
        public int endInt = 0;

        public float startFloat = 0.0f;
        public float endFloat = 0.0f;

        public double startDouble = 0.0;
        public double endDouble = 0.0;
    }

    public class LerpData
    {
        public LerpType lerpType = LerpType.Vector3;
        public IEnumerator lerpRoutine = null;

        public LerpInfos infos = null;
        public LerpPoints points = null;
        public float lerpTime = 0.0f;
        public float elapsedTime = 0.0f;
    }

    public class LerpInfos
    {
        public Vector2 vec2 = Vector2.zero;
        public Vector3 vec3 = Vector3.zero;
        public Quaternion quat = Quaternion.identity;
        public int @int = 0;
        public float @float = 0.0f;
        public double @double = 0.0;

        public float percentage = 0.0f;
        public bool isRunning = false;

        /// <summary>
        /// Reset all infos except coroutine reference
        /// </summary>
        public void ResetInfos()
        {
            vec2 = Vector2.zero;
            vec3 = Vector3.zero;
            quat = Quaternion.identity;
            @int = 0;
            @float = 0.0f;
            @double = 0.0;

            percentage = 0.0f;
            isRunning = false;
        }
    }

    public static class LerpHelper
    {
        /// <summary>
        /// Lerp using lerp type, based on deltaTime
        /// </summary>
        private static IEnumerator LerpRoutine(LerpType lerpType, LerpInfos infos, LerpPoints points, float lerpTime)
        {
            infos.isRunning = true;
            float currentTime = 0.0f;

            if (infos.percentage > 0.0f)
            {
                currentTime = lerpTime * infos.percentage;
            }

            while (currentTime < lerpTime)
            {
                currentTime += Time.deltaTime;
                if (currentTime > lerpTime)
                {
                    currentTime = lerpTime;
                }

                infos.percentage = currentTime / lerpTime;

                switch (lerpType)
                {
                    case LerpType.Vector2:
                        infos.vec2 = Vector2.Lerp(points.startVec2, points.endVec2, infos.percentage);
                        break;

                    case LerpType.Vector3:
                        infos.vec3 = Vector3.Lerp(points.startVec3, points.endVec3, infos.percentage);
                        break;

                    case LerpType.Quaternion:
                        infos.quat = Quaternion.Lerp(points.startQuat, points.endQuat, infos.percentage);
                        break;

                    case LerpType.Int:
                        infos.@int = (int)Mathf.Lerp(points.startInt, points.endInt, infos.percentage);
                        break;

                    case LerpType.Float:
                        infos.@float = Mathf.Lerp(points.startFloat, points.endFloat, infos.percentage);
                        break;

                    case LerpType.Double:
                        infos.@double = points.startDouble + (points.endDouble - points.startDouble) * Mathf.Clamp01(infos.percentage);
                        break;

                    default:
                        Debug.LogError("Not supported lerp type!");
                        break;
                }

                yield return null;
            }

            infos.percentage = 0.0f;
            infos.isRunning = false;
        }

        /// <summary>
        /// Lerp vector2 from [start] to [end] in [time] seconds. If startNow is false, ResumeLerp() function will be needed to start coroutine.
        /// </summary>
        public static LerpData SetLerp(MonoBehaviour owner, Vector2 start, Vector2 end, float time, bool startNow = true)
        {
            LerpInfos infos = new LerpInfos();
            infos.vec2 = start;

            LerpPoints points = new LerpPoints();
            points.startVec2 = start;
            points.endVec2 = end;

            LerpData data = new LerpData();
            data.lerpType = LerpType.Vector2;
            data.infos = infos;
            data.points = points;
            data.lerpTime = time;

            data.lerpRoutine = LerpRoutine(LerpType.Vector2, infos, points, time);

            if (startNow)
            {
                owner.StartCoroutine(data.lerpRoutine);
            }

            return data;
        }

        /// <summary>
        /// Lerp vector3 from [start] to [end] in [time] seconds. If startNow is false, ResumeLerp() function will be needed to start coroutine.
        /// </summary>
        public static LerpData SetLerp(MonoBehaviour owner, Vector3 start, Vector3 end, float time, bool startNow = true)
        {
            LerpInfos infos = new LerpInfos();
            infos.vec3 = start;

            LerpPoints points = new LerpPoints();
            points.startVec3 = start;
            points.endVec3 = end;

            LerpData data = new LerpData();
            data.lerpType = LerpType.Vector3;
            data.infos = infos;
            data.points = points;
            data.lerpTime = time;

            data.lerpRoutine = LerpRoutine(LerpType.Vector3, infos, points, time);

            if (startNow)
            {
                owner.StartCoroutine(data.lerpRoutine);
            }

            return data;
        }

        /// <summary>
        /// Lerp quaternion from [start] to [end] in [time] seconds. If startNow is false, ResumeLerp() function will be needed to start coroutine.
        /// </summary>
        public static LerpData SetLerp(MonoBehaviour owner, Quaternion start, Quaternion end, float time, bool startNow = true)
        {
            LerpInfos infos = new LerpInfos();
            infos.quat = start;

            LerpPoints points = new LerpPoints();
            points.startQuat = start;
            points.endQuat = end;

            LerpData data = new LerpData();
            data.lerpType = LerpType.Quaternion;
            data.infos = infos;
            data.points = points;
            data.lerpTime = time;

            data.lerpRoutine = LerpRoutine(LerpType.Quaternion, infos, points, time);

            if (startNow)
            {
                owner.StartCoroutine(data.lerpRoutine);
            }

            return data;
        }

        /// <summary>
        /// Lerp quaternion from [start] to [end] in [time] seconds. If startNow is false, ResumeLerp() function will be needed to start coroutine.
        /// </summary>
        public static LerpData SetLerp(MonoBehaviour owner, int start, int end, float time, bool startNow = true)
        {
            LerpInfos infos = new LerpInfos();
            infos.@int = start;

            LerpPoints points = new LerpPoints();
            points.startInt = start;
            points.endInt = end;

            LerpData data = new LerpData();
            data.lerpType = LerpType.Int;
            data.infos = infos;
            data.points = points;
            data.lerpTime = time;

            data.lerpRoutine = LerpRoutine(LerpType.Int, infos, points, time);

            if (startNow)
            {
                owner.StartCoroutine(data.lerpRoutine);
            }

            return data;
        }

        /// <summary>
        /// Lerp quaternion from [start] to [end] in [time] seconds. If startNow is false, ResumeLerp() function will be needed to start coroutine.
        /// </summary>
        public static LerpData SetLerp(MonoBehaviour owner, float start, float end, float time, bool startNow = true)
        {
            LerpInfos infos = new LerpInfos();
            infos.@float = start;

            LerpPoints points = new LerpPoints();
            points.startFloat = start;
            points.endFloat = end;

            LerpData data = new LerpData();
            data.lerpType = LerpType.Float;
            data.infos = infos;
            data.points = points;
            data.lerpTime = time;

            data.lerpRoutine = LerpRoutine(LerpType.Float, infos, points, time);

            if (startNow)
            {
                owner.StartCoroutine(data.lerpRoutine);
            }

            return data;
        }

        /// <summary>
        /// Lerp quaternion from [start] to [end] in [time] seconds. If startNow is false, ResumeLerp() function will be needed to start coroutine.
        /// </summary>
        public static LerpData SetLerp(MonoBehaviour owner, double start, double end, float time, bool startNow = true)
        {
            LerpInfos infos = new LerpInfos();
            infos.@double = start;

            LerpPoints points = new LerpPoints();
            points.startDouble = start;
            points.endDouble = end;

            LerpData data = new LerpData();
            data.lerpType = LerpType.Double;
            data.infos = infos;
            data.points = points;
            data.lerpTime = time;

            data.lerpRoutine = LerpRoutine(LerpType.Double, infos, points, time);

            if (startNow)
            {
                owner.StartCoroutine(data.lerpRoutine);
            }

            return data;
        }

        /// <summary>
        /// Resume lerp where it was stopped. If lerp was finished, restart from zero.
        /// </summary>
        public static void ResumeLerp(MonoBehaviour owner, LerpData data)
        {
            if (data.lerpRoutine != null)
            {
                if (!data.infos.isRunning)
                {
                    data.lerpRoutine = LerpRoutine(data.lerpType, data.infos, data.points, data.lerpTime);
                    owner.StartCoroutine(data.lerpRoutine);
                }
                else
                {
                    Debug.LogError("Coroutine is already running!");
                }
            }
            else
            {
                Debug.LogError("LerpInfos has no coroutine to resume!");
            }
        }

        /// <summary>
        /// Stop lerp coroutine. If reset, reset infos except coroutine reference.
        /// </summary>
        public static void StopLerp(MonoBehaviour owner, LerpData data, bool reset = false)
        {
            owner.StopCoroutine(data.lerpRoutine);
            data.infos.isRunning = false;

            if (reset)
            {
                data.infos.ResetInfos();
            }
        }
    }
}
