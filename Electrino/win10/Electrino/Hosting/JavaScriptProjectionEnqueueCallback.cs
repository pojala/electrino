namespace ChakraHost.Hosting
{
    using System;

    /// <summary>
    ///     The context passed into application callback, JsProjectionEnqueueCallback, from Jsrt and
    ///     then passed back to Jsrt in the provided callback, JsProjectionCallback, by the application
    ///     on the correct thread.
    /// </summary>
    /// <remarks>
    ///     Requires calling JsSetProjectionEnqueueCallback to receive callbacks.
    /// </remarks>
    public struct JavaScriptProjectionCallbackContext
    {
        /// <summary>
        /// The reference.
        /// </summary>
        private readonly IntPtr reference;
    }

    /// <summary>
    ///     The Jsrt callback which should be called with the context passed to JsProjectionEnqueueCallback on
    ///     the correct thread.
    /// </summary>
    /// <remarks>
    ///     Requires calling JsSetProjectionEnqueueCallback to receive callbacks.
    /// </remarks>
    /// <param name="jsContext">The context originally received by a call to JsProjectionEnqueueCallback.</param>
    public delegate void JavaScriptProjectionCallback(JavaScriptProjectionCallbackContext jsContext);

    /// <summary>
    ///     The application callback which is called by Jsrt when a projection API is completed on
    ///     a different thread than the original.
    /// </summary>
    /// <remarks>
    ///     Requires calling JsSetProjectionEnqueueCallback to receive callbacks.
    /// </remarks>
    /// <param name="jsCallbck">The callback to be invoked on the original thread.</param>
    /// <param name="callbackState">The applications context.</param>
    /// <param name="jsContext">The Jsrt context that must be passed into jsCallback.</param>
    public delegate void JavaScriptProjectionEnqueueCallback(JavaScriptProjectionCallback jsCallback, JavaScriptProjectionCallbackContext jsContext, IntPtr callbackState);
}
