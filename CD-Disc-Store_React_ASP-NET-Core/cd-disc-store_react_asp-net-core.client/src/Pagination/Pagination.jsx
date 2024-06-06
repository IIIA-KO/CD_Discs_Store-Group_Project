import React from 'react';
import "./Pagination.css"

const Pagination = ({ currentPage, totalPages, onPageClick }) => {
  const pageNumbers = [];
  const maxPageNumbers = 5;
  
  let startPage = 1;
  let endPage = totalPages;
  if (totalPages > maxPageNumbers) {
      const halfMax = Math.floor(maxPageNumbers / 2);
      startPage = Math.max(1, currentPage - halfMax);
      endPage = Math.min(totalPages, startPage + maxPageNumbers - 1);
      if (endPage - startPage + 1 < maxPageNumbers) {
          startPage = Math.max(1, endPage - maxPageNumbers + 1);
      }
  }

  for (let i = startPage; i <= endPage; i++) {
      pageNumbers.push(i);
  }

  const handleClick = (pageNumber) => {
    onPageClick(pageNumber);
  };

  return (
    <div className='pagination'>
      <button disabled={currentPage === 1} onClick={() => handleClick(1)}>First</button>
      <button disabled={currentPage === 1} onClick={() => handleClick(currentPage - 1)}>Prev</button>
      {pageNumbers.map((number) => (
        <button key={number} onClick={() => handleClick(number)} className={currentPage === number ? 'active' : ''}>
          {number}
        </button>
      ))}
      <button disabled={currentPage === totalPages} onClick={() => handleClick(currentPage + 1)}>Next</button>
      <button disabled={currentPage === totalPages} onClick={() => handleClick(totalPages)}>Last</button>
    </div>
  );
};

export default Pagination;