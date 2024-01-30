import React, { useState } from 'react';
import "./Pagination.css"
const Pagination = ({ currentPage, totalPages, onPageClick }) => {
    const pageNumbers = [];
  
    for (let i = 1; i <= totalPages; i++) {
      pageNumbers.push(i);
    }
  
    return (
      <div className='pagination'>
        {pageNumbers.map((number) => (
          <button key={number} onClick={() => onPageClick(number)}>
            {number}
          </button>
        ))}
      </div>
    );
  };

export default Pagination;
