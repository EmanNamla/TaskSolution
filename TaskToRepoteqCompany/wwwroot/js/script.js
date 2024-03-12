window.addEventListener('load', function () {
    const dashLinks = document.querySelectorAll('.sidebar ul li a');

    // Function to handle link click
    function handleLinkClick(e) {
        // Remove active class from all links
        dashLinks.forEach(link => {
            link.classList.remove('active');
        });

        // Add active class to clicked link
        e.target.classList.add('active');
    }

    // Add click event listener to each link
    dashLinks.forEach(link => {
        link.addEventListener('click', handleLinkClick);
    });

    // Optional: Set initial active link based on current URL
    const dashcurrentURL = window.location.href;
    dashLinks.forEach(link => {
        if (link.href === dashcurrentURL) {
            link.classList.add('active');
        }
    });
})
