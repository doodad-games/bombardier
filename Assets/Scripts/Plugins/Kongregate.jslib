mergeInto(LibraryManager.library, {
  KongregateInit: function () {
    if (typeof(kongregateUnitySupport) === 'undefined') {
      return false;
    }

    kongregateUnitySupport.initAPI('Kongregate', 'OnKongregateAPILoaded');

    return true;
  },

  KongregateSubmitStat: function(name, value) {
    kongregate.stats.submit(Pointer_stringify(name), value);
  }
});