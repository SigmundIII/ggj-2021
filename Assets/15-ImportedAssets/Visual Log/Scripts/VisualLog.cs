using System.Collections.Generic;
using UnityEngine;

namespace Visual_Log {
	public enum HorizontalAlignment { Left, Middle, Right }
	public enum VerticalAlignment { Top, Middle, Bottom }
	
	public class VisualLog : MonoBehaviour {
		private static VisualLog instance;

		[Header("DO NOT CHANGE WHILE IN PLAY")]
		[Tooltip("If true Debug logs will be automatically appended.")]
		[SerializeField] private bool attachToDebug = true;
		
		[Header("GUI Settings")]
		#region
		
		[Space]
		[Tooltip("Horizontal alignment of the log window (determines position)")]
		[SerializeField] private HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left;
		[Tooltip("Vertical alignment of the log window (determines position)")]
		[SerializeField] private VerticalAlignment verticalAlignment = VerticalAlignment.Bottom;
		[Space]
		[Tooltip("Show/Hide the background")]
		public bool showBackground;

		#endregion

		[Header("Fading Settings")]
		#region
		
		[Tooltip("If true the logs disappear after a specified amount of time (but won't be deleted)")]
		[SerializeField] private bool logsFading = true;
		[Tooltip("Amount of time before which the fading starts (useful to let the user read well)")]
		[SerializeField] private float logWaitBeforeFading = Log.DefaultWaitTime;
		[Tooltip("Time needed for the text fading to complete")]
		[SerializeField] private float logFadeDuration = Log.DefaultFadeDuration;

		#endregion

		[Header("Control Booleans")]
		#region

		[Tooltip("If the log window is visible on start")]
		[SerializeField] private bool openOnStart = true;
		[Tooltip("Automatic scrolling of the panel when a new message is appended")]
		public bool autoScrollOnNewMessage = true;
		[Tooltip("If the visualization order of the logs should be normal or reversed:\n" +
		         "Normal: newer logs down, older logs up\n" +
		         "Reversed: newer logs up, older logs down")]
		public bool logDisplayReversed = false;
		public bool showLogType;

		#endregion

		[Header("Key Mappings")]
		#region
		
		[Tooltip("If Control key (Left or Right) needs to be pressed with toggle key")]
		public bool ctrl;
		[Tooltip("If Shift key (Left or Right) needs to be pressed with toggle key")]
		public bool shift;
		[Tooltip("If Alt key (Left or Right) needs to be pressed with toggle key")]
		public bool alt;
		[Tooltip("Key with which the window can be opened and closed")]
		public KeyCode toggleKey = KeyCode.F12;
		
		#endregion
		
		[Header("Colors")]
		#region

		public Color infoLogColor = Color.white;
		public Color warningLogColor = Color.yellow;
		public Color errorLogColor = Color.red;

		#endregion
		
		[Header("Optimization")]
		#region

		[Tooltip("Max number of logs that can be registered (when this number is exceeded the older logs are deleted)")]
		[Min(0)] public int maxLogs = 200;

		#endregion

		private List<Log> logs = new List<Log>();

		private Rect logArea = new Rect(0, 0, Screen.width, Screen.height);

		private Vector2 scrollPosition;

		private bool[] pressedToggles;

		#region Area Alignment

		public HorizontalAlignment HorizontalAlignment {
			set {
				float x;
				horizontalAlignment = value;
				switch (value) {
				default:
				case HorizontalAlignment.Left:
					x = 0;
					break;
				case HorizontalAlignment.Middle:
					x = Screen.width / 2 - Screen.width / 2;
					break;
				case HorizontalAlignment.Right:
					x = Screen.width - Screen.height;
					break;
				}
				
				logArea.position = new Vector2(x, logArea.position.y);
			}
		}
		
		public VerticalAlignment VerticalAlignment {
			set {
				float y;
				verticalAlignment = value;
				switch (value) {
				default:
				case VerticalAlignment.Top:
					y = 0;
					break;
				case VerticalAlignment.Middle:
					y = Screen.height / 2 - Screen.height / 2;
					break;
				case VerticalAlignment.Bottom:
					y = Screen.height - Screen.height;
					break;
				}
				
				logArea.position = new Vector2(logArea.position.x, y);
			}
		}

		#endregion

		#region Width - Height properties

		public int Width {
			set => logArea.size = new Vector2(value, logArea.size.y);
		}
		
		public int Height {
			set => logArea.size = new Vector2(logArea.size.x, value);
		}

		#endregion

		private bool IsOpen { get; set; }
		
		public static bool IsWindowOpen => instance.IsOpen;

		#region Awake - Start

		private void Awake() {
			if (instance == null) {
				DontDestroyOnLoad(gameObject);
				instance = this;
			} else {
				Destroy(gameObject);
			}
		}

		private void Start() {
			if (openOnStart) {
				IsOpen = true;
			}
			
			UpdateGUI();
		}

		#endregion

		private void Update() {
			bool ctrlPressed = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
			bool shiftPressed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
			bool altPressed = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
			bool toggleKeyPressed = Input.GetKeyDown(toggleKey);
			if (toggleKeyPressed) {
				bool toggle = true;
				if (ctrl) {
					toggle = ctrlPressed && toggle;
				}

				if (shift) {
					toggle = shiftPressed && toggle;
				}

				if (alt) {
					toggle = altPressed && toggle;
				}

				if (toggle) {
					IsOpen = !IsOpen;
				}
			}

			if (Application.isEditor && Input.GetKeyDown(KeyCode.U)) {
				UpdateGUI();
			}
		}

		private void UpdateGUI() {
			Width = Screen.width;
			Height = Screen.height;
			HorizontalAlignment = horizontalAlignment;
			VerticalAlignment = verticalAlignment;
		}

		public void AddLogMessage(string condition, string stacktrace, LogType type) {
			var log = new Log(this, type, condition, stacktrace);
			StartCoroutine(log.FadeOut(logWaitBeforeFading, logFadeDuration));
			logs.Add(log);
			if (logs.Count > maxLogs) {
				logs.RemoveAt(0);
			}
			if (autoScrollOnNewMessage) {
				scrollPosition += Vector2.up * 1000;
			}
		}

		private void OnGUI() {
			if (IsOpen) {
				var style = new GUIStyle("label");
				if (logDisplayReversed) {
					style.alignment = TextAnchor.UpperLeft;
				} else {
					style.alignment = TextAnchor.LowerLeft;
				}
				
				if (showBackground) {
					GUI.Box(logArea, GUIContent.none, GUI.skin.box);
				}
				
				GUILayout.BeginArea(logArea);
				scrollPosition = GUILayout.BeginScrollView(scrollPosition);

				if (!logDisplayReversed) {
					GUILayout.FlexibleSpace();
				}

				var logsToDisplay = new List<Log>(logs);
				if (logDisplayReversed) {
					logsToDisplay.Reverse();
				}
				
				foreach (Log log in logsToDisplay) {
					if (!logsFading || log.Visible) {
						log.OnGUI(style, !logsFading);
					}
				}

				if (logDisplayReversed) {
					GUILayout.FlexibleSpace();
				}

				GUILayout.EndScrollView();
				GUILayout.EndArea();
			}
		}
		
		private void Clear() {
			logs.Clear();
		}

		#region Static Functions

		/// <summary>
		/// Append a log to the visual log window (doesn't also use Debug.Log)
		/// </summary>
		/// <param name="condition">Message of the log</param>
		/// <param name="stacktrace">Exception or error stacktrace (if not setted, the error stacktrace will not be displayed)</param>
		/// <param name="logType">Indicates the type of the log (info, warning, error, assertion, stacktrace)</param>
		public static void AddLog(string condition) {
			instance.AddLogMessage(condition, "", LogType.Log);
		}

		/// <summary>
		/// Deletes all the logs from the window
		/// </summary>
		public static void ClearWindow() {
			instance.Clear();
		}

		#endregion

		#region Enable - Disable

		private void OnEnable() {
			if (attachToDebug) {
				Application.logMessageReceived += AddLogMessage;
			}
		}

		private void OnDisable() {
			if (attachToDebug) {
				Application.logMessageReceived -= AddLogMessage;
			}
		}

		#endregion
	}

}

