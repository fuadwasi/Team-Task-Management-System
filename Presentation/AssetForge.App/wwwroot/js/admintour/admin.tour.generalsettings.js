$(function() {
  const tour = new Shepherd.Tour(AdminTourCommonTourOptions);

  AdminTourNextPageButton.action = function () { window.location = '/Admin/site/Edit/' + AdminTourDataProvider.next_button_entity_id + '?showtour=True' };

  //'Welcome' step
  tour.addStep({
    title: AdminTourDataProvider.localized_data.PersonalizeSiteIntroTitle,
    text: AdminTourDataProvider.localized_data.PersonalizeSiteIntroText,
    buttons: [AdminTourNextButton]
  });

  //'Basic/Advanced mode' step
  tour.addStep({
    title: AdminTourDataProvider.localized_data.PersonalizeSiteBasicAdvancedTitle,
    text: AdminTourDataProvider.localized_data.PersonalizeSiteBasicAdvancedText,
    attachTo: {
      element: '.onoffswitch',
      on: 'auto'
    },
    buttons: [AdminTourBackButton, AdminTourNextButton]
  });

  //'Choose a theme' step
  tour.addStep({
    title: AdminTourDataProvider.localized_data.PersonalizeSiteThemeTitle,
    text: AdminTourDataProvider.localized_data.PersonalizeSiteThemeText,
    attachTo: {
      element: '#theme-area',
      on: 'auto'
    },
    buttons: [AdminTourBackButton, AdminTourNextButton]
  });

  //'Upload your logo' step
  tour.addStep({
    title: AdminTourDataProvider.localized_data.PersonalizeSiteLogoTitle,
    text: AdminTourDataProvider.localized_data.PersonalizeSiteLogoText,
    attachTo: {
      element: '#logo-area',
      on: 'auto'
    },
    buttons: [AdminTourBackButton, AdminTourNextPageButton]
  });

  tour.start();
})