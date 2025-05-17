$(function() {
  const tour = new Shepherd.Tour(AdminTourCommonTourOptions);

  //'Title and content' step
  tour.addStep({
    title: AdminTourDataProvider.localized_data.PageTitleContentTitle,
    text: AdminTourDataProvider.localized_data.PageTitleContentText,
    attachTo: {
      element: '#info-area',
      on: 'bottom'
    },
    buttons: [AdminTourNextButton]
  });

  //'Preview the page' step
  tour.addStep({
    title: AdminTourDataProvider.localized_data.PagePreviewTitle,
    text: AdminTourDataProvider.localized_data.PagePreviewText,
    attachTo: {
      element: '#preview-page-button',
      on: 'bottom'
    },
    buttons: [AdminTourBackButton, AdminTourDoneButton]
  });

  tour.start();
})