const track = document.querySelector(".marquee-content");

track.innerHTML += track.innerHTML;


const input = document.querySelector("input");

input.addEventListener("focus", () => {
    const main = document.querySelector("main");
    const content = document.querySelector(".marquee-container");

    main.classList.add("remove");
    content.classList.add("remove");
});
input.addEventListener("blur", () => {
    const main = document.querySelector("main");
    const content = document.querySelector(".marquee-container");

    main.classList.remove("remove");
    content.classList.remove("remove");
});

const form = document.getElementById("form");

form.addEventListener("submit", async(event) => {
    event.preventDefault();

    const bookName = document.getElementById("search").value;

    try {
        const response = await fetch('http://localhost:5169/home/search-books?bookName=' + bookName);

        const books = await response.json();
        console.log(books);
        const container = document.getElementById("container-books");
        container.innerHTML = "";

        books.forEach(element => {
            const bookCard = document.createElement("div");

            bookCard.innerHTML = `<h2> ${books} <h2>`;
            container.appendChild(bookCard);
        });
    }
    catch (error)
    {
        alert(error);
    }
})