import React, { useState, useEffect } from 'react';
import "./FilmSearch.css"
import CardListFilm from '../pages/CardListFilm/CardListFilm'

const FilmSearch = ({ films, currentPage, itemsPerPage, onUpdateTotalPages }) => {
  const [searchText, setSearchText] = useState('');
  const [genreFilter, setGenreFilter] = useState('');
  const [genres, setGenres] = useState([]);
  const [searchResults, setSearchResults] = useState([]);
  const [totalItems, setTotalItems] = useState(0);

  useEffect(() => {
    const fetchGenres = async () => {
      try {
        const response = await fetch(`https://localhost:7117/Film/GetGenres`);
        const data = await response.json();
        setGenres(data);
      } catch (error) {
        console.error(error);
      }
    };

    fetchGenres();
  }, [searchText]);

  useEffect(() => {
    const fetchSearchResults = async () => {
      try {
        let url = `https://localhost:7117/Film/GetAll?searchText=${searchText}&skip=${(currentPage - 1) * itemsPerPage}&take=${itemsPerPage}`;
        const response = await fetch(url);
        const data = await response.json();
        setSearchResults(data.items);
        setTotalItems(data.countItems);
        onUpdateTotalPages(Math.ceil(data.countItems / itemsPerPage));
      } catch (error) {
        console.error(error);
      }
    };
    fetchSearchResults();
  }, [searchText, genreFilter, films, currentPage, itemsPerPage, onUpdateTotalPages]);

  const handleSearchChange = (e) => {
    setSearchText(e.target.value);
  }

  const handleGenreChange = async (e) => {
    setSearchText(e.target.value);
  };

  return (
    <div>
      <div className='search_bar'>
        <input type="text" placeholder='Search films' value={searchText} onChange={handleSearchChange} />
        <select value={genreFilter} onChange={handleGenreChange}>
          <option value="">All genres</option>
          {
            genres.map(genre => (
              <option key={genre} value={genre}>{genre}</option>
            ))
          }
        </select>
      </div>
      <CardListFilm data={searchResults} />
    </div>
  );
};

export default FilmSearch;
