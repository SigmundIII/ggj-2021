using System.Collections;
using UnityEngine;

namespace VisualLog {

	public class Log {
		public const float DefaultWaitTime = 2f;
		public const float DefaultFadeDuration = 1.5f;

		private VisualLog visualLog;
		
		private LogType type;
		private string message;
		private string stacktrace;

		private float alpha = 1;

		public bool Visible => alpha > 0;

		public Log(VisualLog visualLog, LogType type, string message, string stacktrace) {
			this.visualLog = visualLog;
			this.type = type;
			this.message = message;
			this.stacktrace = stacktrace;
		}

		public void OnGUI(GUIStyle style, bool forceAlphaTo1 = false) {
			string log;
			Color color;
			
			switch (type) {
			default:
				log = $"{message}";
				color = visualLog.infoLogColor;
				break;
			}

			if (forceAlphaTo1) {
				color.a = 1;
			} else {
				color.a = alpha;
			}
			style.normal.textColor = color;
			style.fontSize = 35;
			GUILayout.Label(log, style);
		}

		public IEnumerator FadeOut(float waitDelta, float fadeDuration) {
			float timer = waitDelta;
			while (timer > 0) {
				timer -= Time.deltaTime;
				yield return null;
			}

			yield return null;
			
			timer = fadeDuration;
			while (timer > 0) {
				timer -= Time.deltaTime;
				alpha = timer / fadeDuration;
				yield return null;
			}

			alpha = 0;
		}
	}

}