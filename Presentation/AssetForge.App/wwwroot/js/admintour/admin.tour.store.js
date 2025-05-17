$(function() {
  const tour = new Shepherd.Tour(AdminTourCommonTourOptions);

  AdminTourNextPageButton.action = function () { window.location = '/Admin/Shipping/Providers?showtour=True' };

  //'Your store name' step
  tour.addStep({
    title: AdminTourDataProvider.localized_data.SiteNameTitle,
    text: AdminTourDataProvider.localized_data.SiteNameText,
    attachTo: {
      element: '#store-name-area',
      on: 'bottom'
    },
    buttons: [AdminTourNextButton]
  });

  //'Your store URL' step
  tour.addStep({
    title: AdminTourDataProvider.localized_data.SiteUrlTitle,
    text: AdminTourDataProvider.localized_data.SiteUrlText,
    attachTo: {
      element: '#store-url-area',
      on: 'bottom'
    },
    buttons: [AdminTourBackButton, AdminTourNextButton]
  });

  //'Enable SSL' step
  tour.addStep({
    title: AdminTourDataProvider.localized_data.SiteSslTitle,
    text: AdminTourDataProvider.localized_data.SiteSslText,
    attachTo: {
      element: '#ssl-area',
      on: 'bottom'
    },
    buttons: [AdminTourBackButton, AdminTourNextPageButton]
  });

  tour.start();
})