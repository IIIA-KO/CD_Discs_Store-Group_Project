import React, { useState, useEffect } from 'react';
import "./MusicSearch.css"
import CardListMusic from '../pages/CardListMusic/CardListMusic';

const MusicSearch = ({ musics, currentPage, itemsPerPage }) => {
  const [searchText, setSearchText] = useState('');
  const [genreFilter, setGenreFilter] = useState('');
  const [genres, setGenres] = useState([]);
  const [searchResults, setSearchResults] = useState([]);

  useEffect(() => {
    const fetchGenres = async () => {
      try {
        const response = await fetch(`https://localhost:7117/Music/GetGenres`);
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
        const response = await fetch(`https://localhost:7117/Music/GetAll?searchText=${searchText}&skip=${(currentPage - 1) * itemsPerPage}&take=${itemsPerPage}`);
        const data = await response.json();
        setSearchResults(data.items);
      } catch (error) {
        console.error(error);
      }
    };
    fetchSearchResults();
  }, [searchText, genreFilter, musics]);

  const handleSearchChange = (e) => {
    setSearchText(e.target.value);
  }

  const handleGenreChange = async (e) => {
    setSearchText(e.target.value);
  };

  return (
    <div>
      <div className='search_bar'>
        <input type="text" placeholder='search musics' value={searchText} onChange={handleSearchChange} />
        <select value={genreFilter} onChange={handleGenreChange}>
          <option value="">All genres</option>
          {
            genres.map(genre => (
              <option key={genre} value={genre}>{genre}</option>
            ))
          }
        </select>
      </div>

      <CardListMusic data={searchResults} />
    </div>
  );
};

export default MusicSearch;