namespace VirtueSky.Core
{
    public struct CoreEnum
    {
        public enum RuntimeAutoInitType
        {
            /// <summary>
            ///   <para>Callback invoked when the first scene's objects are loaded into memory and after Awake has been called.</para>
            /// </summary>
            AfterSceneLoad,

            /// <summary>
            ///   <para>Callback invoked when the first scene's objects are loaded into memory but before Awake has been called.</para>
            /// </summary>
            BeforeSceneLoad,
        }
    }
}