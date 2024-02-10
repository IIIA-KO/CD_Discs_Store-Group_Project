import React from 'react'
import { useEffect, useState } from 'react'
import MusicSearch from '../../MusicSearch/MusicSearch';
import Pagination from '../../Pagination/Pagination';

const Music = () => {
  const [musics, setMusics] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(0);
  const itemsPerPage = 12;

  useEffect(() => {
    const fetchMusics = async () => {
      try {
        const response = await fetch(`https://localhost:7117/Music/GetAll?skip=${(currentPage - 1) * itemsPerPage}&take=${itemsPerPage}`);
        const data = await response.json();
        setMusics(data.items);
        setTotalPages(Math.ceil(data.countItems / itemsPerPage));
      } catch (error) {
        console.error(error);
      }
    };

    fetchMusics();
  }, [currentPage]);

  const handlePageClick = (pageNumber) => {
    setCurrentPage(pageNumber);
  };

  const handleTotalPagesUpdate = (total) => {
    setTotalPages(total);
  };

  return (
    <div>
      <MusicSearch
        musics={musics}
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

export default Music;