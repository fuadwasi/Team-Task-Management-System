$(function() {
  $('#pages-grid').on('draw.dt', function () {
    const tour = new Shepherd.Tour(AdminTourCommonTourOptions);

    AdminTourNextPageButton.action = function () { window.location = '/Admin/Page/Edit/' + AdminTourDataProvider.next_button_entity_id + '?showtour=True' };

    //'Pages (pages)' step
    tour.addStep({
      title: AdminTourDataProvider.localized_data.PageListPages1Title,
      text: AdminTourDataProvider.localized_data.PageListPages1Text,
      attachTo: {
        element: '#pages-area',
        on: 'bottom'
      },
      buttons: [AdminTourNextButton]
    });

    var pageRowId = 'row_shippinginfo';

    if ($('#' + pageRowId).length) {
      //'Pages (pages)' step
      tour.addStep({
        title: AdminTourDataProvider.localized_data.PageListPages2Title,
        text: AdminTourDataProvider.localized_data.PageListPages2Text,
        attachTo: {
          element: '#pages-area',
          on: 'bottom'
        },
        buttons: [AdminTourBackButton, AdminTourNextButton]
      });

      //'Page row' step
      tour.addStep({
        title: AdminTourDataProvider.localized_data.PageListShippingTitle,
        text: AdminTourDataProvider.localized_data.PageListShippingText,
        attachTo: {
          element: '#' + pageRowId,
          on: 'bottom'
        },
        buttons: [AdminTourBackButton, AdminTourNextButton]
      });

      //'Link location' step
      tour.addStep({
        title: AdminTourDataProvider.localized_data.PageListLocationTitle,
        text: AdminTourDataProvider.localized_data.PageListLocationText,
        attachTo: {
          element: '#' + pageRowId + ' .column-footer-column1',
          on: 'bottom'
        },
        buttons: [AdminTourBackButton, AdminTourNextButton]
      });

      //'Edit the page' step
      tour.addStep({
        canClickTarget: true,
        title: AdminTourDataProvider.localized_data.PageListEditTitle,
        text: AdminTourDataProvider.localized_data.PageListEditText,
        attachTo: {
          element: '#' + pageRowId + ' .column-edit .btn',
          on: 'bottom'
        },
        buttons: [AdminTourBackButton, AdminTourNextPageButton]
      });
    } else {
      //'Pages (pages)' step
      tour.addStep({
        title: AdminTourDataProvider.localized_data.PageListPages2Title,
        text: AdminTourDataProvider.localized_data.PageListPages2Text,
        attachTo: {
          element: '#pages-area',
          on: 'bottom'
        },
        buttons: [AdminTourBackButton, AdminTourDoneButton]
      });
    }

    tour.start();
  });
})