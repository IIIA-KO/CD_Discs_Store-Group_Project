import React, { useState, useEffect } from 'react';
import "./MovieSearch.css"
import CardList from '../pages/CardList/CardList';
const MovieSearch = ({ movies }) => {
    const [search, setSearch] = useState('');
  const [genreFilter, setGenreFilter] = useState('');
  const [filteredMovies, setFilteredMovies] = useState([]);

  useEffect(() => {
    const filtered = movies.filter((movie) => {
      const titleMatch = movie.name.toLowerCase().includes(search.toLowerCase());
      const genreMatch = movie.genre.toLowerCase().includes(genreFilter.toLowerCase());
      return titleMatch && genreMatch;
    });
    setFilteredMovies(filtered);
  }, [search, genreFilter, movies]);

  const handleSearchChange = (e) => {
    setSearch(e.target.value);
  }

  const handleGenreChange = (e) => {
    setGenreFilter(e.target.value);
  };


  return (
    <div>
        <div className='search_bar'>
      <input type="text" placeholder='search films' value={search} onChange={handleSearchChange} />
      <select value={genreFilter} onChange={handleGenreChange}>
        <option value="">All genres</option>
        <option value="action">Action</option>
        <option value="comedy">Comedy</option>
        <option value="classic">Classic</option>
        <option value="musical">Musical</option>
        <option value="animation">Animation</option>
        <option value="independent">Independent</option>
        <option value="documentary">Documentary</option>
        <option value="drama">Drama</option>
        <option value="foreign">Foreign</option>
        <option value="romance">Romance</option>
        {/* Другие жанры */}
      </select>
      </div>
      {/* Отображение отфильтрованных результатов */}
      <ul>
        {/*{filteredMovies.map(movie => (
          <li key={movie.id}>{movie.name} - {movie.genre}</li>
        ))}*/}
        <CardList data={filteredMovies} />
      </ul>
    </div>
  );
};

export default MovieSearch;
