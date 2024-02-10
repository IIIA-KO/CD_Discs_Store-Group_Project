import React, { useEffect, useState } from 'react';
import FilmSearch from '../../FilmSearch/FilmSearch';
import Pagination from '../../Pagination/Pagination';

const Films = () => {
  const [films, setFilms] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(0);
  const itemsPerPage = 12;

  useEffect(() => {
    const fetchMovies = async () => {
      try {
        const response = await fetch(`https://localhost:7117/Film/GetAll?skip=${(currentPage - 1) * itemsPerPage}&take=${itemsPerPage}`);
        const data = await response.json();
        setFilms(data.items);
        setTotalPages(Math.ceil(data.countItems / itemsPerPage));
      } catch (error) {
        console.error(error);
      }
    };

    fetchMovies();
  }, [currentPage]);

  const handlePageClick = (pageNumber) => {
    setCurrentPage(pageNumber);
  };

  const handleTotalPagesUpdate = (total) => {
    setTotalPages(total);
  };

  return (
    <div>
      <FilmSearch
        films={films}
        currentPage={currentPage}
        itemsPerPage={itemsPerPage}
        onUpdateTotalPages={handleTotalPagesUpdate} />
      <Pagination
        currentPage={currentPage}
        totalPages={totalPages}
        onPageClick={handlePageClick}
      />
    </div>
  );
};

export default Films;
