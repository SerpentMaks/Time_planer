// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", () => {
    const body = document.body;

    
    body.style.opacity = '0';
    body.style.transform = 'translateY(50px)';
    body.style.transition = 'opacity 0.4s ease, transform 0.4s ease';

  
    requestAnimationFrame(() => {
        body.style.opacity = '1';
        body.style.transform = 'translateY(0)';
    });

  
    document.querySelectorAll('a[href]:not([target="_blank"])').forEach(link => {
        link.addEventListener('click', (e) => {
            const href = link.getAttribute('href');
            if (!href || href.startsWith('#')) return;

            e.preventDefault();

            body.style.opacity = '0';
            body.style.transform = 'translateY(-50px)';

            setTimeout(() => {
                window.location.href = href;
            }, 300); 
        });
    });


    document.querySelectorAll('.card').forEach(card => {
        card.addEventListener('mouseenter', () => {
            card.style.transform = 'translateY(-3px)';
            card.style.boxShadow = '0 8px 24px rgba(0, 0, 0, 0.12)';
            card.style.transition = 'all 0.3s ease';
        });

        card.addEventListener('mouseleave', () => {
            card.style.transform = 'translateY(0)';
            card.style.boxShadow = '0 6px 20px rgba(0, 0, 0, 0.08)';
        });
    });

    window.showNotification = function (message, duration = 4000) {
        const notification = document.getElementById("notification");
        if (!notification) return;

        notification.textContent = message;
        notification.style.display = "block";
        notification.style.animation = "fadeInOut 5s forwards";

        setTimeout(() => {
            notification.style.display = "none";
        }, duration);
    };
});
document.addEventListener("DOMContentLoaded", function () {
    const rowsPerPage = 7;
    const table = document.querySelector("table tbody");
    const rows = Array.from(table.rows);
    const totalRows = rows.length;
    const totalPages = Math.ceil(totalRows / rowsPerPage);
    let currentPage = 1;

    function updateTable() {
        const start = (currentPage - 1) * rowsPerPage;
        const end = start + rowsPerPage;

        rows.forEach(row => row.style.display = "none");

        for (let i = start; i < end && i < totalRows; i++) {
            rows[i].style.display = "";
        }
    }

    function updatePagination() {
        const pagination = document.querySelector("#pagination");
        pagination.innerHTML = "";

        const maxVisiblePages = 5; 

        let startPage = Math.max(currentPage - 1, 1);
        let endPage = Math.min(startPage + maxVisiblePages - 1, totalPages);

        
        if (endPage - startPage < maxVisiblePages - 1) {
            startPage = Math.max(endPage - maxVisiblePages + 1, 1);
        }

        
        if (startPage > 1) {
            createPageButton(1);
            if (startPage > 2) createEllipsis();
        }

        
        for (let i = startPage; i <= endPage; i++) {
            createPageButton(i);
        }

        
        if (endPage < totalPages) {
            if (endPage < totalPages - 1) createEllipsis();
            createPageButton(totalPages);
        }

        function createPageButton(page) {
            const button = document.createElement("button");
            button.textContent = page;
            button.classList.add("pagination-button");
            if (page === currentPage) {
                button.classList.add("active");
            }
            button.addEventListener("click", function () {
                currentPage = page;
                updateTable();
                updatePagination();
            });
            pagination.appendChild(button);
        }

        function createEllipsis() {
            const span = document.createElement("span");
            span.textContent = "...";
            span.classList.add("ellipsis");
            pagination.appendChild(span);
        }
    }

   
    updateTable();
    updatePagination();
});
document.addEventListener("DOMContentLoaded", function () {
    const toggleButton = document.getElementById("toggleUserForm");
    const addUserSection = document.getElementById("addUserSection");

    toggleButton.addEventListener("click", () => {
        const isShown = addUserSection.classList.contains("show");

        if (isShown) {
            addUserSection.classList.remove("show");
            toggleButton.textContent = "Новый пользователь";
        } else {
            addUserSection.classList.add("show");
            toggleButton.textContent = "Скрыть форму";
        }
    });
});
document.addEventListener("DOMContentLoaded", function () {
    const toggleLeaveBtn = document.getElementById("toggleLeaveForm");
    const leaveForm = document.getElementById("leaveRequestForm");

    toggleLeaveBtn.addEventListener("click", () => {
        const isVisible = leaveForm.classList.contains("show");

        if (isVisible) {
            leaveForm.classList.remove("show");
            toggleLeaveBtn.textContent = "Новая заявка на отпуск";
        } else {
            leaveForm.classList.add("show");
            toggleLeaveBtn.textContent = "Скрыть форму";
        }
    });
});
